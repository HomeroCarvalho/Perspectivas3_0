using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Testes;
using MathNet.Numerics.LinearAlgebra;
using System.Drawing;

namespace ProjetoMatrizesNaoQuadraticas.bibliotecasMatematicas
{
    public class MyVetor
    {
        public List<double> data { get; set; }

        public  Color cor { get; set; }
        public  double X
        {
            get
            {
                return data[0];
            }
            set
            {
                data[0] = value;
            }
        }

        public double Y
        {
            get
            {
                return data[1];
            }
            set
            {
                data[1] = value;
            }
        }

        public double Z
        {
            get
            {
                return data[2];
            }
            set
            {
                data[2] = value;
            }
        }


        public static int precisaoDigitos = 21;

        public static bool isDebug = false;

        public MyVetor(int length)
        {
            this.data = new List<double>();
            for (int x = 0; x < length; x++)
                this.data.Add(0);
            this.cor = Color.Black;
        }


        public MyVetor(Color cor, params double[] coordenadas)
        {
            if (coordenadas == null)
                throw new NullReferenceException("coordenadas de vetor nulo.");

            this.cor = Color.FromArgb(cor.A, cor);
            this.data = coordenadas.ToList<double>();
        }



        public MyVetor(params double[] coordenadas)
        {
            if (coordenadas == null)
                throw new NullReferenceException("coordenadas de vetor nulo.");

            this.cor = Color.Black;
            this.data = coordenadas.ToList<double>();
        }




        public int lenght()
        {
            if ((data == null) || (data.Count == 0))
                throw new Exception("vetor nulo, tentando obter seu numero de coordenadas");
            else
                return data.Count;
        }



        public MyVetor Clone()
        {
            MyVetor vt_result = new MyVetor(this.lenght());
            vt_result.data = this.data.ToList<double>();
            vt_result.cor = Color.FromArgb(this.cor.A, this.cor);

            return vt_result;
        }


        public double Module()
        {
            if (data == null)
                throw new NullReferenceException("vetor nulo em calculo de seu modulo!");
            else
            {
                double modulo = 0.0;
                for (int x = 0; x < this.lenght(); x++)
                    modulo += data[x] * data[x];
                return Math.Sqrt(modulo);
            }
        }



        public static MyVetor Add(MyVetor v1, MyVetor v2)
        {
            if ((v1 == null) || (v2 == null))
                throw new NullReferenceException("vetor em adicao nulo.");
            if (v1.lenght() != v2.lenght())
                throw new Exception("vetores em adicao de diferentes tipos.");


            MyVetor vt_result = new MyVetor(v1.lenght());
            
            for (int x = 0; x < v1.lenght(); x++)
                vt_result.data[x] = v1.data[x] + v2.data[x];

            vt_result.cor = Color.FromArgb(v1.cor.A, v1.cor);
            return vt_result;

        }
        public static MyVetor Sub(MyVetor v1, MyVetor v2)
        {
            if ((v1 == null) || (v2 == null))
                throw new NullReferenceException("vetor em adicao nulo.");
            if (v1.lenght() != v2.lenght())
                throw new Exception("vetores em adicao de diferentes tipos.");


            MyVetor vt_result = new MyVetor(v1.lenght());

            for (int x = 0; x < v1.lenght(); x++)
                vt_result.data[x] = v1.data[x] - v2.data[x];

            vt_result.cor = Color.FromArgb(v1.cor.A, v1.cor);
            return vt_result;

        }


        public MyVetor Mul(MyVetor vt_second)
        {
            return Mul(this, vt_second);
        }

        public static MyVetor Mul(MyVetor v, Matriz m)
        {
            if (m == null)
                throw new Exception("matriz multiplicativa nula!");

            if (v.lenght() != m.lines)
                throw new Exception("matrix and vectors incompatibles for multiplication!");


            Matriz mt_vector = ToMatriz(v);
            Matriz mt_result = Matriz.Mul(mt_vector, m);

            MyVetor vt_result = mt_result.ToVetor();
            vt_result.cor = Color.FromArgb(v.cor.A, v.cor);

            return vt_result;
        }

        public static MyVetor Mul(MyVetor v1, MyVetor v2)
        {
            if ((v1 == null) || (v2 == null))
                throw new NullReferenceException("vetor em multiplicacao nulo!");
            if ((v1.data == null) || (v2.data == null))
                throw new NullReferenceException("vetor com coordenacao nulo!");
            

            if (v1.lenght() != v2.lenght())
                throw new Exception("vetores de diferentes tipos, com quantidade de coordenadas diferentes!");



            MyVetor vt_result = new MyVetor(v1.lenght());
            for (int x = 0; x < v1.lenght(); x++)
                vt_result.data[x] = v1.data[x] * v2.data[x];



            vt_result.cor = Color.FromArgb(v1.cor.A, v1.cor);


            return vt_result;
        }


        /// <summary>
        /// rotaciona um vetor na sequencia: planoXY,planoXZ,planoYZ.
        /// A sequencia pode não ser a padronizada para rotações de ângulos Eulier.
        /// </summary>
        public  MyVetor Rotate( double inc_angleXZDegrees, double inc_angleXYDegrees, double inc_angleYZDegrees)
        {
            if (this.lenght() != 3)
                throw new Exception("Metodo MyVetor.Rotate somente para vetores 3d!");

            MyVetor vector_to_rotate = new MyVetor(this.lenght());
            if (inc_angleXYDegrees != 0.0)
            {
                /// 	mt_rotateXY= [[cos(incBeta), -sin(incBeta, 0 ], [sin(incBeta],cos(incBeta),0],[0, 0 ,1]]
                double cosBeta_inc = Math.Cos((Math.PI / 180.0) * inc_angleXYDegrees);
                double sinBeta_inc = Math.Sin((Math.PI / 180.0) * inc_angleXYDegrees);

                Matriz mt_rotateXY = new Matriz(3, 3);
                mt_rotateXY.data[0, 0] = +cosBeta_inc;
                mt_rotateXY.data[0, 1] = -sinBeta_inc;
                mt_rotateXY.data[0, 2] = 0.0;

                mt_rotateXY.data[1, 0] = +sinBeta_inc;
                mt_rotateXY.data[1, 1] = +cosBeta_inc;
                mt_rotateXY.data[1, 2] = 0.0;

                mt_rotateXY.data[2, 0] = 0.0;
                mt_rotateXY.data[2, 1] = 0.0;
                mt_rotateXY.data[2, 2] = 1.0;

                vector_to_rotate = Matriz.Mul(ToMatriz(this), mt_rotateXY).ToVetor();
                vector_to_rotate.cor = Color.FromArgb(this.cor.A, this.cor);
            }

            if (inc_angleXZDegrees != 0.0)
            {
                double cosBeta_inc = Math.Cos((Math.PI / 180.0) * inc_angleXZDegrees);
                double sinBeta_inc = Math.Sin((Math.PI / 180.0) * inc_angleXZDegrees);

                /// mt_rotateXZ=[[cos(incBeta), 0, -sin(incBeta),0],[0,1,0], [sin(incBeta],0,cos(incBeta),]]
                Matriz mt_rotateXZ = new Matriz(3, 3);
                mt_rotateXZ.data[0, 0] = +cosBeta_inc;
                mt_rotateXZ.data[0, 1] = 0.0;
                mt_rotateXZ.data[0, 2] = -sinBeta_inc;

                mt_rotateXZ.data[1, 0] = 0.0;
                mt_rotateXZ.data[1, 1] = 1.0;
                mt_rotateXZ.data[1, 2] = 0.0;

                mt_rotateXZ.data[2, 0] = +sinBeta_inc;
                mt_rotateXZ.data[2, 1] = 0.0;
                mt_rotateXZ.data[2, 2] = +cosBeta_inc;

                vector_to_rotate = Matriz.Mul(ToMatriz(this), mt_rotateXZ).ToVetor();
                vector_to_rotate.cor = Color.FromArgb(this.cor.A, this.cor); 
            }
            if (inc_angleYZDegrees != 0.0)
            {
                double cosBeta_inc = Math.Cos((Math.PI / 180.0) * inc_angleYZDegrees);
                double sinBeta_inc = Math.Sin((Math.PI / 180.0) * inc_angleYZDegrees);

                ///mt_rotateYZ=[[1,0,0],[0, cos(incBeta), -sin(incBeta)],[0, sin(incBeta],cos(incBeta)]]
                Matriz mt_rotateYZ = new Matriz(3, 3);
                mt_rotateYZ.data[0, 0] = 1.0;
                mt_rotateYZ.data[0, 1] = 0.0;
                mt_rotateYZ.data[0, 2] = 0.0;

                mt_rotateYZ.data[1, 0] = 0.0;
                mt_rotateYZ.data[1, 1] = +cosBeta_inc;
                mt_rotateYZ.data[1, 2] = -sinBeta_inc;

                mt_rotateYZ.data[2, 0] = 0.0;
                mt_rotateYZ.data[2, 1] = +sinBeta_inc;
                mt_rotateYZ.data[2, 2] = +cosBeta_inc;


                vector_to_rotate = Matriz.Mul(ToMatriz(this), mt_rotateYZ).ToVetor();
                vector_to_rotate.cor = Color.FromArgb(this.cor.A, this.cor);
            }

            return vector_to_rotate;
        }

        public MyVetor Rotate(MyVetor vector_inAround, double inc_planoXY,double inc_planoXZ, double inc_planoYZ)
        {
            MyVetor vector_rotated = this.Clone();
            MyVetor vt_nomalize = vector_inAround.Clone().Normaliza();

            double X = vt_nomalize.X;
            double Y = vt_nomalize.Y;
            double Z = vt_nomalize.Z;


            /// plano XY: (X,Y,0) e vector arround.
            double anguloXY_vectorArround = Math.Atan2(Y, X) * 180 / Math.PI;

            /// plano XZ: (X,0,Z) e vector arround.
            double anguloXZ_vectorArround = Math.Atan2(Z,X) * 180 / Math.PI;


            /// plano YZ: (0,Y,Z) e vector arround.
            double anguloYZ_vectorArround = Math.Atan2(Y,Z) * 180 / Math.PI;



            vector_rotated = vector_rotated.Rotate(+anguloXZ_vectorArround, +anguloXY_vectorArround, +anguloYZ_vectorArround);
            vector_rotated = vector_rotated.Rotate(+inc_planoXZ, +inc_planoXY, +inc_planoYZ);
            vector_rotated = vector_rotated.Rotate(-anguloXZ_vectorArround, -anguloXZ_vectorArround, -anguloYZ_vectorArround);


            return vector_rotated;
        }
        
        public MyVetor Mul(double number)
        {
            if ((this.data == null) || (data.Count == 0))
                return null;
            else
            {
                MyVetor vt_result = this.Clone();
                for (int x = 0; x < vt_result.lenght(); x++)
                    vt_result.data[x] *= number;

                vt_result.cor = Color.FromArgb(this.cor.A, this.cor);
                return vt_result;
            }
            
            
        }

      
        public void PrintVector(string caption)
        {
            string str_precisao = "N" + MyVetor.precisaoDigitos.ToString();

            int x = 0;
            System.Console.Write(caption + "  ");
            System.Console.Write("[ ");
            if (!isDebug)
            {
                for (x = 0; x < this.lenght() - 1; x++)
                    System.Console.Write(Math.Round(this.data[x]).ToString(str_precisao) + ",");
                System.Console.WriteLine(Math.Round(this.data[x]).ToString(str_precisao) + " ]");
                return;
            }
            else
            if (isDebug)
            {
                for (x = 0; x < this.lenght() - 1; x++)
                    System.Console.Write(this.data[x].ToString(str_precisao) + ",");
                System.Console.WriteLine(this.data[x].ToString(str_precisao) + " ]");
                return;
            }
        }

        public static MyVetor Normaliza(MyVetor vt_to_normalize)
        {
            return vt_to_normalize.Normaliza();
        }

        public void Normalizacao()
        {
            double modulo = 0.0;
            for (int x = 0; x < this.data.Count; x++)
                modulo += this.data[x] * this.data[x];

            modulo = Math.Sqrt(modulo);

            for (int x = 0; x < data.Count; x++)
                this.data[x] = this.data[x] / modulo;
        }
        public MyVetor Normaliza()
        {
            MyVetor v_to_normalize = new MyVetor(lenght());
            double modulo = 0.0;
            for (int x = 0; x < data.Count; x++)
                modulo += data[x] * data[x];

            modulo = Math.Sqrt(modulo);

            for (int x = 0; x < data.Count; x++)
                v_to_normalize.data[x] = data[x] / modulo;

            v_to_normalize.cor = Color.FromArgb(this.cor.A, this.cor);
            return v_to_normalize;
              
        }

        public static Matriz ToMatriz(MyVetor v)
        {
            Matriz mt_vetor = new Matriz(1, v.lenght());


            for (int x = 0; x < v.lenght(); x++)
                mt_vetor.data[0, x] = v.data[x];

            return mt_vetor;
        }



        public override string ToString()
        {
            
            string str = "[";
            
            int x = 0;
            for (x = 0; x < this.lenght()-1; x++)
                str += this.data[x] + ",";
            str += this.data[x];

            str += "]";

            return str;
        }


        public class Testes : SuiteClasseTestes
        {
            public Testes() : base("teste para funcionalidades classe MyVetor")
            {
            }

            public void TesteMultVetor(AssercaoSuiteClasse assercao)
            {
                MyVetor v1 = new MyVetor(1, 2, 3);
                MyVetor v2 = new MyVetor(2, 2, 2);

                MyVetor vt_result = MyVetor.Mul(v1, v2);

                v1.PrintVector("vetor 1:");
                v2.PrintVector("vetor 2: ");
                vt_result.PrintVector("vetor resultante");

                assercao.IsTrue(Math.Abs(vt_result.data[2] - 6) < 0.1);

                System.Console.ReadLine();
            }

            public void TesteRotacoesVetorEmTornoDeOutroVetor(AssercaoSuiteClasse assercao)
            {
                double inc_angle = 45.0;
                MyVetor.isDebug = true;
                



                MyVetor vt_in_arround1 = new MyVetor(0, 1, 0);
                MyVetor vt_to_rotate1 = new MyVetor(1, 0, 0);
                vt_to_rotate1 = vt_to_rotate1.Rotate(vt_in_arround1, inc_angle, 0, 0);




                MyVetor vt_in_arround2 = new MyVetor(0, 1, 0);
                MyVetor vt_to_rotate2 = new MyVetor(1, 0, 0);
                vt_to_rotate2 = vt_to_rotate2.Rotate(vt_in_arround2, 0, inc_angle, 0);






                vt_in_arround1.PrintVector("vetor rotationado em torno deste ângulo");
                vt_to_rotate1.PrintVector("vetor rotacionado, em torno do vetor.");


                vt_in_arround2.PrintVector("vetor rotationado em torno deste ângulo");
                vt_to_rotate2.PrintVector("vetor rotacionado, em torno do vetor.");


                assercao.IsTrue(Math.Abs(vt_to_rotate1.Z - Math.Cos(inc_angle * Math.PI / 180.0)) < 0.01);
                assercao.IsTrue(Math.Abs(vt_to_rotate2.X - Math.Cos(inc_angle * Math.PI / 180.0)) < 0.01);

                System.Console.ReadLine();
            }


            public void TesteRotacoesVetor(AssercaoSuiteClasse assercao)
            {
                double inc_angleTeste = -45.0;
                MyVetor.isDebug = true;

                MyVetor vt_test = new MyVetor(1.0, 0.0, 0.0);
                MyVetor vt_test_rotateXY = vt_test.Rotate(0.0, inc_angleTeste, 0.0);
                

                vt_test.PrintVector("vetor inicial: ");
                vt_test_rotateXY.PrintVector("vetor rotacionado " + inc_angleTeste.ToString("N2") + " graus no plano XY");

                assercao.IsTrue(Math.Abs(vt_test_rotateXY.X - Math.Cos((Math.PI / 180) * inc_angleTeste)) < 0.1);
       


                MyVetor vt_teste_rotateXZ = vt_test.Rotate(inc_angleTeste, 0.0, 0.0);
                vt_teste_rotateXZ.PrintVector("vetor rotacionado " + inc_angleTeste.ToString("N2") + " graus no planoXZ");

                assercao.IsTrue(Math.Abs(vt_teste_rotateXZ.Z - Math.Cos((Math.PI / 180) * inc_angleTeste)) < 0.1);
        



                MyVetor vt_test_rotateYZ = vt_test.Rotate(0.0, 0.0, inc_angleTeste);
                vt_test_rotateYZ.PrintVector("vetor rotacionado " + inc_angleTeste.ToString("N2") + " graus no planoYZ");

                assercao.IsTrue(Math.Abs(vt_test_rotateYZ.X - 1.0) < 0.1);
                System.Console.ReadLine();



                System.Environment.Exit(1);
            }
        }
    }

}
