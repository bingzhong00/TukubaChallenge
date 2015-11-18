
using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace LocationPresumption
{
    class KalmanFilter
    {
        /// <summary>
        /// シンプル　カルマンフィルタ
        /// </summary>
        public class SimpleKalman
        {
	        const double Q = 0.0001;
	        const double R = 0.01;
	        double P, X, K;

            public SimpleKalman()
            {
		        P = 0.0;
                X = 0.0;
            }

	        public double update(double measurement)
	        {
		        measurementUpdate();

		        double result = X + (measurement - X) * K;
		        X = result;
		        return result;
	        }

	        private void measurementUpdate()
	        {
		        K = (P + Q) / (P + Q + R);
		        P = R * (P + Q) / (R + P + Q);
	        }
        }

        /// <summary>
        /// シンプル　ローパスフィルタ
        /// </summary>
        public class SimpleLPF
        {
	        const int win_size = 16;

	        double[] data = new double[win_size];
	        double total;
	        int pointer;

	        public SimpleLPF(double initMeasurement=0.0 )
            {
                total = initMeasurement * win_size;
                pointer = (0);

		        for(int i=0;i<win_size;i++)
                    data[i] = initMeasurement;
	        }

	        public double update(double measurement)
	        {
		        total -= data[pointer];
		        total += measurement;
		        
                data[pointer] = measurement;
		        pointer++;

		        if(pointer==win_size) pointer = 0;
		        return total/win_size;
	        }

            public double value()
            {
                return total / win_size;
            }
        }

        /// <summary>
        /// ハイパスフィルタ
        /// </summary>
        public class SimpleHPF
        {
	        SimpleLPF lpf = new SimpleLPF();

            public SimpleHPF() { }

	        public double update(double measurement)
	        {
		        return measurement - lpf.update(measurement);
	        }
        }

        /// <summary>
        /// テストデータ作成
        /// </summary>
        /// <param name="size"></param>
        /// <param name="myu"></param>
        /// <param name="sigma"></param>
        /// <returns></returns>
        public double[] genGaussArray(int size, double myu, double sigma)
        {
	        double[] a = new double[size];
            Random Rand = new Random();

	        if(a == null)
            {
		        //printf("There is not an enough memory!\n");
		        //exit(1);
                throw new ApplicationException("There is not an enough memory!");
	        }

            for(int i=0;i<size-1;i+=2)
            {
		        double tmp0 = Rand.NextDouble();
		        double tmp1 = Rand.NextDouble();
                double sgm = sigma * Math.Sqrt(-2 * Math.Log(tmp0));

                a[i] = myu + sgm * Math.Sin(2 * Math.PI * tmp1);
                a[i + 1] = myu + sgm * Math.Cos(2 * Math.PI * tmp1);
	        }

	        return a;
        }

#if false
//#define MAX(a,b) (((a)>(b))?(a):(b))

        int main(int argc, char **argv)
        {
	        const static int num = 256;
	        SimpleKalman sk;
	        SimpleLPF slpf;
	        SimpleHPF shpf;

	        srand((unsigned)time(NULL));
	        double *dat = genGaussArray(num, 10.0, 0.4);

	        printf("div, ");
	        for (int i = 0; i < num; i++)
	        {
		        printf("%d, ", i);
	        }
	        printf("\n");
	        // Check Gaussian distribution array
	        printf("Gauss Array, ");
	        for (int i = 0; i < num; i++)
	        {
		        printf("%.2f, ", dat[i]);
	        }
	        printf("\n");
        /*
	        // Make histogram to check Gaussian distribution.
	        printf("Histogram:\n");
	        const static int hist_num = 32;
	        int hist[hist_num] ={0};
	        for (int i = 0; i < num; i++)
	        {
		        hist[(int)ceil(dat[i]*(hist_num>>1))-1]++;
	        }
	        int hist_max = 0;
	        for (int i = 0; i < hist_num; i++)
	        {
		        if(hist_max < hist[i]) hist_max = hist[i];
		        //printf("%d, ", hist[i]);
	        }
	        int hist_div = MAX(hist_max / hist_num, 1);
	        for (int i = 0; 0 < hist_max; i++)
	        {
		        hist_max -= hist_div;
		        for (int i = 0; i < hist_num; i++)
		        {
			        if(hist_max <= hist[i]) printf("o");
			        else printf(" ");
		        }
		        printf("\n");
	        }
	        printf("\n");
        */
	        printf("Simple Kalman Filter, ");
	        for (int i = 0; i < num; i++)
	        {
		        //if(i%64==0)
			        printf("%.2f, ", sk.update(dat[i]));
	        }
	        printf("\n");

	        printf("Simple Low-pass Filter, ");
	        for (int i = 0; i < num; i++)
	        {
		        //if(i%64==0)
			        printf("%.2f, ", slpf.update(dat[i]));
	        }
	        printf("\n");
        /*
	        printf("Simple High-pass Filter, ");
	        for (int i = 0; i < num; i++)
	        {
		        printf("%.2f, ", shpf.update(dat[i]));
	        }
	        printf("\n");
        */
	        free(dat);

	        return 0;
        }
#endif
    }
}
