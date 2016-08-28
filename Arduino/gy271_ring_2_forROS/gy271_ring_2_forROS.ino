/*
An Arduino code example for interfacing with the HMC5883
 
by: Jordan McConnell
 SparkFun Electronics
 created on: 6/30/11
 license: OSHW 1.0, http://freedomdefined.org/OSHW
 
Analog input 4 I2C SDA
Analog input 5 I2C SCL
*/
#include <Adafruit_NeoPixel.h>
#ifdef __AVR__
  #include <avr/power.h>
#endif

#define PIN 6

Adafruit_NeoPixel strip = Adafruit_NeoPixel(60, PIN, NEO_GRB + NEO_KHZ800);

#include <Wire.h> //I2C Arduino Library
 
#define address 0x1E //0011110b, I2C 7bit address of HMC5883

uint32_t dirG = strip.Color(0,16,0);
uint32_t dirB = strip.Color(0,0,16);
uint32_t dirR = strip.Color(16,0,0);

int xmin, ymin, xmax, ymax;
int ofx, ofy;
int dcnt = 0;

int sw = 0;

void showDir(uint32_t c, uint8_t n, uint8_t wait) {

  int8_t l;

  l = 0;  
  //Serial.print(n);
  //Serial.print(",");
  //Serial.println(l);
  
  strip.setPixelColor(n, c);    //turn every third pixel o
  strip.setPixelColor(l, dirB);    //turn every third pixel o
  if (n == 0){
    strip.setPixelColor(n, dirG);    //turn every third pixel o
  }
  strip.show();
  delay(wait);
  strip.setPixelColor(n, 0);        //turn every third pixel off     
  strip.setPixelColor(l, 0);        //turn every third pixel off     
}

// Fill the dots one after the other with a color
void colorWipe(uint32_t c, uint8_t wait) {
  for(uint16_t i=0; i< 16; i++) {
    strip.setPixelColor(i, c);
  }
  strip.show();
}

void colorSmileFace(uint32_t c, uint8_t wait) {

  int f = 9;
  int t = 0;
  uint32_t dt;
  int ss;
  ss = f;
  for(uint16_t i=0; i <16; i++)
  {
    if (ss == f) dt = c;
    if (ss == t) dt = 0; 
    strip.setPixelColor(ss, dt);
    ss++;
    ss %= 16;
    strip.show();
  }
}

void setup()
{
  #if defined (__AVR_ATtiny85__)
    if (F_CPU == 16000000) clock_prescale_set(clock_div_1);
  #endif

  ofx = 0;
  ofy = 0;

  xmin=9999;
  ymin=9999;
  xmax=-9999;
  ymax=-9999;
  
  pinMode(2,INPUT);
  
  //Initialize Serial and I2C communications
  //Serial.begin(9600);
  Serial.begin(57600);
  Wire.begin();
   
  //Put the HMC5883 IC into the correct operating mode
  Wire.beginTransmission(address); //open communication with HMC5883
  Wire.write(0x02); //select mode register
  Wire.write(0x00); //continuous measurement mode
  Wire.endTransmission();

  strip.begin();
  strip.show();
}

 //Theatre-style crawling lights.
void theaterChase(uint32_t c, uint8_t wait) {
  for (int j=0; j<10; j++) {  //do 10 cycles of chasing
    for (int q=0; q < 3; q++) {
      for (int i=0; i < 16; i=i+3) {
        strip.setPixelColor(i+q, c);    //turn every third pixel on
      }
      strip.show();

      delay(wait);

      for (int i=0; i < 16; i=i+3) {
        strip.setPixelColor(i+q, 0);        //turn every third pixel off
      }
    }
  }
}

void loop()
{
  int dir, n, l;   
  int x,y,z; //triple axis data
  int xsum, ysum;

  xsum = 0;
  ysum = 0;

  n = 0;

  while (1)
  {
    // Tell the HMC5883 where to begin reading data
    Wire.beginTransmission(address);
    Wire.write(0x03); //select register 3, X MSB register
    Wire.endTransmission();   
    
    // Read data from each axis, 2 registers per axis
    Wire.requestFrom(address, 6);
    if (6 <= Wire.available())
    {
      x = Wire.read()<<8; //X msb
      x |= Wire.read(); //X lsb
      z = Wire.read()<<8; //Z msb
      z |= Wire.read(); //Z lsb
      y = Wire.read()<<8; //Y msb
      y |= Wire.read(); //Y lsb

      xsum += x;
      ysum += y;
      n++;
    }
    
    if (n >= 5) break;
  }

  x = xsum / 5;
  y = ysum / 5;
  
  if (x < xmin) xmin = x;
  if (y < ymin) ymin = y;
  if (xmax < x) xmax = x;
  if (ymax < y) ymax = y;
   
  //Print out values of each axis
  Serial.print("magAxiss: ");
  Serial.print(x);
  Serial.print(",");
  Serial.print(y);
  Serial.print(",");
  Serial.println(z);
   
  //if (sw == 0) delay(100);
  
  dir = atan2( y-ofy , x-ofx ) * (180.0 / 3.141592);

  dir += 7; // henkaku 

  if (dir < 0) dir += 360;
  if (dir > 360) dir -= 360;  

//  Serial.print ("dir = ");
    dcnt++;
    //Serial.print(dcnt);
    //Serial.print (",");

  //Serial.println (dir);

  n = (int) (((float) (dir) + 11.25) / 22.5) % 16;
  
  if (digitalRead(2) == HIGH)
  {
    Serial.print(xmin);
    Serial.print (",");
    Serial.print(xmax);
    Serial.print (",");
    Serial.print(ymin);
    Serial.print (",");
    Serial.println(ymax);

    ofx = (xmin + xmax) / 2;
    ofy = (ymin + ymax) / 2;  
  }

  if (Serial.available())
  {
    char inChar = (char) Serial.read();
    sw = inChar - 0x30;
  }

  switch (sw) 
  {
    case 1:
      colorWipe(strip.Color(255, 0, 0), 1); // Red
      break;
    case 2:
      colorWipe(strip.Color(0, 255, 0), 1); // Green
      break;
    case 3:
      colorWipe(strip.Color(0, 0, 255), 1); // Blue
      break;
    case 4:
      colorSmileFace(strip.Color(255, 0, 255), 1); // Purple
      break;
    case 5:
      theaterChase(strip.Color(127, 127, 127), 50); // White
      break;
    case 6:  
      theaterChase(strip.Color(127, 0, 0), 50); // Red
      break;
    case 7:
      theaterChase(strip.Color(0, 127, 0), 50); // Green
      break;
    case 8:
      theaterChase(strip.Color(0, 0, 127), 50); // Blue
      break;
      
    default:
      showDir(dirR,n,100);
      break;
   }
   
}


