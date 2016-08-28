#!/usr/bin/env python
# -*- coding: utf-8 -*-
# シリアルLED動作テスト
import RPi.GPIO as GPIO
import time
from neopixel import *	#serial Led import

#-------------------------------------------------------
#シリアルLED　定義
#-------------------------------------------------------
# LED strip configuration:
S_LED_COUNT      = 16      # Number of LED pixels.
S_LED_PIN        = 18      # GPIO pin connected to the pixels (must support PWM!).
S_LED_FREQ_HZ    = 800000  # LED signal frequency in hertz (usually 800khz)
S_LED_DMA        = 5       # DMA channel to use for generating signal (try 5)
S_LED_BRIGHTNESS = 255     # Set to 0 for darkest and 255 for brightest
S_LED_INVERT     = False   # True to invert the signal (when using NPN transistor level shift)
S_LED_STATUS_SUCCESS 	= 1
S_LED_STATUS_BAD	= 0
S_LED_NIKO_EYE_LEFT = 2 		#ニコマークの左目
S_LED_NIKO_EYE_RIGHT = 14 		#ニコマークの右目
S_LED_NIKO_MOUTH = 6 		#ニコマークの口

S_LED_DESIGN_PTN_NOW_LOADING 	= 1  #起動中
S_LED_DESIGN_PTN_WAITING 	= 2  #待機中（非自立モード)
S_LED_DESIGN_PTN_MOVING_LRF	= 3  #ルート動作中（主にLRF)
S_LED_DESIGN_PTN_MOVING_NON_LRF	= 4  #ルート動作中（LRF以外)
S_LED_DESIGN_PTN_NOT_READY	= 5  #通信途絶、動作不能
S_LED_DESIGN_PTN_FRONT_AVOIDANCE= 6  #前方障害物回避中(停止)
S_LED_DESIGN_PTN_L_R_AVOIDANCE  = 7  #左右壁　障害物回避中(壁よけ)
S_LED_DESIGN_PTN_PERSON_SEARCH 	= 8  #人探し中
S_LED_DESIGN_PTN_GOAL	 	= 9  #ゴール
S_LED_DESIGN_PTN_STOPLIGHT 	= 10 #信号待ち
S_LED_DESIGN_PTN_TURNING	= 11 #その場旋回中

#-------------------------------------------------------
#シリアルLED　グローバル変数
#-------------------------------------------------------
gslStartNo = 0		#シリアルLEDの初期位置
gslActiveNo = 0 	#現在アクティブなLEDNo
gslDesignPtnOld = 0 	#過去の点灯パターン
gslFlashFlg = 0     	#点滅フラグ
gslAddNo = 1		#LEDNoの加算方向
gslRandomFlashCnt = 0	#ゴール時のランダム点灯
#-------------------------------------------------------
# インスタンス実体化
#-------------------------------------------------------
strip = Adafruit_NeoPixel(S_LED_COUNT, S_LED_PIN, S_LED_FREQ_HZ, S_LED_DMA, S_LED_INVERT, S_LED_BRIGHTNESS)
#-------------------------------------------------------
# シリアルLED関連機能
#-------------------------------------------------------
def init_serial_led():
	# Intialize the library (must be called once before other functions).
	strip.begin()

#-------------------------------------------------------
#サークルの基準番号の変更
# No:基点となるLEDNo
#-------------------------------------------------------
def slSetStartNo(No):
	global gslStartNo
	if  0 > No:
		No = 0 
	elif S_LED_COUNT <= No:
		No = S_LED_COUNT - 1
	gslStartNo = No

#-------------------------------------------------------
#LEDに点灯するLEDのNoと色情報を伝える
#slNo:点灯するLEDのNo
#slColor:色情報
#-------------------------------------------------------
def slSetPixelColor(slNo,slColor):
	global gslStartNo
	slNewNo = gslStartNo+slNo
	if S_LED_COUNT <= slNewNo:
		slNewNo -= S_LED_COUNT
	strip.setPixelColor(slNewNo,slColor)

#-------------------------------------------------------
#LEDクリア
#-------------------------------------------------------
def slClear():
	for i in range(S_LED_COUNT):
		slSetPixelColor(i, Color(0, 0, 0))

#-------------------------------------------------------
#カラーパターン1 起動中
#GaugeCnt:読み込み具合（0〜100）
#ColorPtn:色情報
#-------------------------------------------------------
def slColorPtn1(GaugeCnt,ColorPtn):
	for i in range(GaugeCnt):			
		slSetPixelColor(i, ColorPtn)

#-------------------------------------------------------
#カラーパターン2 待機中（非自立モード)
#ActiveNo:点灯する(アクティブな)LEDNo
#ColorPtn:色情報
#-------------------------------------------------------
def slColorPtn2(ActiveNo,ColorPtn):
	slSetPixelColor(ActiveNo,ColorPtn)
	ActiveNo2 = ActiveNo+8
	ActiveNo2 = slActiveNoOverChk(ActiveNo2)
	slSetPixelColor(ActiveNo2, ColorPtn)

#-------------------------------------------------------
#カラーパターン3 ルート動作中（主にLRF)
#ActiveNo:点灯する(アクティブな)LEDNo
#ColorPtn:色情報
#-------------------------------------------------------
def slColorPtn3(ActiveNo,ColorPtn):
	ActiveNo2 =  slActiveNoOffsetClac(ActiveNo,-1)
	ActiveNo3 =  slActiveNoOffsetClac(ActiveNo,-2)

	ColorPtn2 = slColorDataHalf(ColorPtn)
	ColorPtn3 = slColorData1bit(ColorPtn)

	slSetPixelColor(ActiveNo, ColorPtn)
	slSetPixelColor(ActiveNo2, ColorPtn2)
	slSetPixelColor(ActiveNo3, ColorPtn3)

#-------------------------------------------------------
#カラーパターン4 ルート動作中（LRF以外)
#ActiveNo:点灯する(アクティブな)LEDNo
#ColorPtn:色情報
#-------------------------------------------------------
def slColorPtn4(ActiveNo,ColorPtn):
	ans = ActiveNo
	for i in range(S_LED_COUNT/2+1):
		slSetPixelColor(ans, ColorPtn)
		ans = ans + 2
		ans = slActiveNoOverChk(ans)

#-------------------------------------------------------
#カラーパターン5 通信途絶、動作不能
#ActiveNo:点灯する(アクティブな)LEDNo
#ColorPtn:色情報
#-------------------------------------------------------
def slColorPtn5(ActiveNo,ColorPtn):
	for i in range(S_LED_COUNT):
		slSetPixelColor(i, ColorPtn)

#-------------------------------------------------------
#カラーパターン6,7 前方障害物回避中(停止),左右壁　障害物回避中(壁よけ)
#ActiveNo:点灯する(アクティブな)LEDNo
#ColorPtn:色情報
#DirectionFlg:点滅する方向(縦or横)
#-------------------------------------------------------
def slColorPtn6_7(ActiveNo,ColorPtn,DirectionFlg):
	if 1 == DirectionFlg:	
		DirectionNo = 4	#左右点滅
	else:
		DirectionNo = 0	#上下点滅
	ans = ActiveNo % 2
	if 0 == ans:	
		HalfNo = 0
	else:
		HalfNo = 8
	for i in range(S_LED_COUNT/2+1):
		No = slActiveNoOverChk(i + HalfNo + DirectionNo)
		slSetPixelColor(No, ColorPtn)

#-------------------------------------------------------
#カラーパターン8 人探し中
#ColorPtn:色情報
#-------------------------------------------------------
def slColorPtn8(ColorPtn):
	slSetPixelColor(S_LED_NIKO_EYE_LEFT, ColorPtn)
	slSetPixelColor(S_LED_NIKO_EYE_RIGHT, ColorPtn)
	for i in range(S_LED_NIKO_MOUTH):
		slSetPixelColor(i+5, ColorPtn)

#-------------------------------------------------------
#カラーパターン9 ゴール
#-------------------------------------------------------
def slColorPtn9():
	global gslRandomFlashCnt
	RedColor = Color(0,255,0)
	GreenColor = Color(255,0,0)
	BuleColor = Color(0,0,255)
	
	if 0 == gslRandomFlashCnt:
		NowColorPtn = RedColor
	elif 1 == gslRandomFlashCnt:
		NowColorPtn = GreenColor
	else:		
		NowColorPtn = BuleColor
	gslRandomFlashCnt = gslRandomFlashCnt + 1
	if 3 <= gslRandomFlashCnt:
		gslRandomFlashCnt = 0

	slColorPtn8(NowColorPtn)

#-------------------------------------------------------
#カラーパターン10 信号待ち
#ActiveNo:点灯する(アクティブな)LEDNo
#-------------------------------------------------------
def slColorPtn10(ActiveNo):
	Red = Color(0,255,0)
	Green = Color(255,0,0)
	NowColor = Red
	for i in range(S_LED_COUNT):
		ans = slActiveNoOverChk(i+ActiveNo)
		slSetPixelColor(ans, NowColor)
		ChgColorFlg = i % 4
		if 0 == ChgColorFlg:
			if NowColor == Red:
				NowColor = Green
			else:
				NowColor = Red

#-------------------------------------------------------
#カラーパターン11 その場旋回中
#ActiveNo:点灯する(アクティブな)LEDNo
#-------------------------------------------------------
def slColorPtn11(ActiveNo):
	ans = ActiveNo % 2
	if 0 == ans:
		Orange = Color(120,255,0)
		for i in range(4):	
			slSetPixelColor(i*4+2, Orange)

#-------------------------------------------------------
# 現在のアクティブなLEDNoに対し、オフセットしたNoを算出する
# ActiveNo:アクティブなLEDNo
# Cnt:オフセット値
#-------------------------------------------------------
def slActiveNoOffsetClac(ActiveNo,Cnt):
	ans = Cnt % S_LED_COUNT
	ans = ActiveNo + ans;
	if 0 > ans:
		ans = S_LED_COUNT + ans
	elif S_LED_COUNT <= ans:
		ans = ans - S_LED_COUNT 

	return ans

#-------------------------------------------------------
#現在のLEDNoがオーバーフローしていないか確認
#ActiveNo:点灯する(アクティブな)LEDNo
#-------------------------------------------------------
def slActiveNoOverChk(ActiveNo):
	if 0 > ActiveNo:
		ActiveNo = S_LED_COUNT + ActiveNo
	elif S_LED_COUNT <= ActiveNo:
		ActiveNo = ActiveNo - S_LED_COUNT
	return ActiveNo

#-------------------------------------------------------
#起動中の読み込み具合の計算
#Gauge:読み込み具合（0〜100）
#-------------------------------------------------------
def slGaugeClac(Gauge):
	if 0 > Gauge:
		Gauge = 0
	elif 100 < Gauge:
		Gauge = 100			
	return int(Gauge / 100.0 * S_LED_COUNT)

#-------------------------------------------------------
#LEDの点灯パターン振り分け
#DesignPtn:点灯パターン
#ColorPtn:色情報
#Gauge:読み込み具合(0〜100)
#-------------------------------------------------------
def slSelectColorPtn(DesignPtn,ColorPtn,Gauge):
	global gslActiveNo
	global gslDesignPtnOld

	if DesignPtn != gslDesignPtnOld:
		gslActiveNo = 0
	gslDesignPtnOld = DesignPtn

	if S_LED_DESIGN_PTN_NOW_LOADING == DesignPtn: #起動中
		GaugeCnt = slGaugeClac(Gauge) #読み込みゲージをLEDに変換
		slColorPtn1(GaugeCnt,ColorPtn)
	elif S_LED_DESIGN_PTN_WAITING == DesignPtn: #待機中（非自立モード)
		slColorPtn2(gslActiveNo,ColorPtn)
	elif S_LED_DESIGN_PTN_MOVING_LRF == DesignPtn: #ルート動作中（主にLRF)
		slColorPtn3(gslActiveNo,ColorPtn)
	elif S_LED_DESIGN_PTN_MOVING_NON_LRF == DesignPtn: #ルート動作中（LRF以外)
		slColorPtn4(gslActiveNo,ColorPtn)
	elif S_LED_DESIGN_PTN_NOT_READY == DesignPtn: #通信途絶、動作不能
		slColorPtn5(gslActiveNo,ColorPtn)
	elif S_LED_DESIGN_PTN_FRONT_AVOIDANCE == DesignPtn or S_LED_DESIGN_PTN_L_R_AVOIDANCE == DesignPtn: #前方障害物回避中(停止),左右壁　障害物回避中(壁よけ)
		if S_LED_DESIGN_PTN_FRONT_AVOIDANCE == DesignPtn:
			DirectionFlg = 0
		else:
			DirectionFlg = 1
		slColorPtn6_7(gslActiveNo,ColorPtn,DirectionFlg)
	elif S_LED_DESIGN_PTN_PERSON_SEARCH ==  DesignPtn: #人探し中
		slColorPtn8(ColorPtn)
	elif S_LED_DESIGN_PTN_GOAL ==  DesignPtn: #ゴール
		slColorPtn9()		
	elif S_LED_DESIGN_PTN_STOPLIGHT ==  DesignPtn: #信号待ち
		slColorPtn10(gslActiveNo)
	elif S_LED_DESIGN_PTN_TURNING ==  DesignPtn: #その場旋回中
		slColorPtn11(gslActiveNo)
	
	gslActiveNo = slActiveNoClac(DesignPtn,gslActiveNo)

#-------------------------------------------------------
#アクティブなLEDの番号の算出
#DesignPtn:点灯パターン
#ActiveNo:点灯する(アクティブな)LEDNo
#-------------------------------------------------------
def slActiveNoClac(DesignPtn,ActiveNo):
	global gslAddNo
	if S_LED_DESIGN_PTN_WAITING == DesignPtn:
		if(S_LED_COUNT / 2) <= ActiveNo:
			gslAddNo = -1
		elif 0 == ActiveNo:
			gslAddNo = 1
	else:
		gslAddNo = 1
		
	ActiveNo = ActiveNo + gslAddNo
	return slActiveNoOverChk(ActiveNo)

#-------------------------------------------------------
# LEDの点灯、点滅処理
#DesignPtn:点灯パターン
#ColorPtn:色情報
#FlashPtn:点滅の有無
#Gauge:読み込み具合(0〜100）
#-------------------------------------------------------
def slFlashSerialLed(DesignPtn,ColorPtn,FlashPtn,Gauge):
	global gslFlashFlg

	slClear()
	if ((S_LED_DESIGN_PTN_MOVING_LRF == DesignPtn) | (S_LED_DESIGN_PTN_MOVING_NON_LRF==DesignPtn) ) & ( 0 == FlashPtn): #点滅
		if gslFlashFlg == 0:
			gslFlashFlg = 1 
			slSelectColorPtn(DesignPtn,ColorPtn,Gauge)
		else:
			gslFlashFlg = 0 
	else: #点灯
		slSelectColorPtn(DesignPtn,ColorPtn,Gauge)
	strip.show()

#-------------------------------------------------------
#カラーデータ1/2化
#ColorPtn:色情報
#-------------------------------------------------------
def slColorDataHalf(ColorPtn):
	gdata = (ColorPtn & 0xFF0000) >> 16
	rdata = (ColorPtn & 0x00FF00) >> 8
	bdata = (ColorPtn & 0x0000FF)
	gdatahalf = gdata / 2
	rdatahalf = rdata / 2
	bdatahalf = bdata / 2
	rgbdata = gdatahalf << 16 & 0xFF0000 	
	rgbdata |= rdatahalf << 8 & 0x00FF00 	
	rgbdata |= bdatahalf 
	return rgbdata

#-------------------------------------------------------
#カラーデータ1bit化
#ColorPtn:色情報
#-------------------------------------------------------
def slColorData1bit(ColorPtn):
	gdata = (ColorPtn & 0xFF0000) >> 16
	rdata = (ColorPtn & 0x00FF00) >> 8
	bdata = (ColorPtn & 0x0000FF)
	gdata1bit = 0
	rdata1bit = 0
	bdata1bit = 0
	if 0 < gdata:
		gdata1bit = 1
	if 0 < rdata:
		rdata1bit = 1
	if 0 < bdata:
		bdata1bit = 1
	rgbdata = ((gdata1bit << 16) & 0xFF0000) 	
	rgbdata |= ((rdata1bit << 8) & 0x00FF00) 	
	rgbdata |= bdata1bit 
	return rgbdata

#-------------------------------------------------------
# メイン処理
#-------------------------------------------------------
def main():
	init_serial_led()
 	slSetStartNo(2)		#LEDの基点設定

	ptn_cnt = 0
	lp_cnt = 0

	slDesignPtn = 2	#点灯パターン 1〜11
	slColorPtn = 0	#カラーパターン 0:白 1:赤 2:緑 3:青
	slFlashFlg = 1	#点滅有無 0:点滅 1:点灯
	slGauge = 0

	RedColor = Color(0,255,0)
	GreenColor = Color(255,0,0)
	BuleColor = Color(0,0,255)
	OrangeColor = Color(120,255,0)

	while ptn_cnt < 1:
		while lp_cnt <=32:
			#デザインパターン呼び出し
			slFlashSerialLed(slDesignPtn,RedColor,slFlashFlg,slGauge)
			time.sleep(0.1)               # 0.1秒待機
			lp_cnt=lp_cnt+1
			slGauge = slGauge + 1
		slDesignPtn = slDesignPtn + 1
		slGauge = 0
		ptn_cnt = ptn_cnt+1 	
		lp_cnt = 0

#        GPIO.cleanup()                      # GPIOピンの設定解除
if __name__ == "__main__":
    main()



