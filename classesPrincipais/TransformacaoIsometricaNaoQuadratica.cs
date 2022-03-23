using System;
using System.Collections.Generic;
using System.Text;
using MatrizesNaoQuadraticas;
using ModuloTESTES;
using MathNet.Numerics.LinearAlgebra;
using System.Drawing;
using Testes;

namespace Isometria

{



    public class TransformacaoIsometricaNaoQuadratica
    {
        private Matrizes m32;
        private Matrizes m23;

        public TransformacaoIsometricaNaoQuadratica()
        {
            this.m32 = this.M32();
            this.m23 = this.M23(m32);
        }

        private static TransformacaoIsometricaNaoQuadratica transformacaoSingleton = null;
        public static TransformacaoIsometricaNaoQuadratica Instance()
        {
            if (transformacaoSingleton == null)
                transformacaoSingleton = new TransformacaoIsometricaNaoQuadratica();
            return transformacaoSingleton;
        }

        private Matrizes M32()
        {
            Matrizes m32 = new Matrizes(3, 2);
            m32.SetElement(0, 0, 1.0);
            m32.SetElement(1, 0, 0.0);
            m32.SetElement(2, 0, 0.3);

            m32.SetElement(0, 1, 0.0);
            m32.SetElement(1, 1, 1.0);
            m32.SetElement(2, 1, 0.3);

            return m32;

        }

        public Matrix<double> M32_23_inv(double c1)
        {
            Matrix<double> k = Matrix<double>.Build.Dense(3, 3);
            k[2, 0] = 0.0;
            k[2, 1] = 0.0;
            k[2, 2] = 1.0;

            return (this.m32.matrix * this.m23.matrix + k).Inverse();
        }

        private Matrizes M23(Matrizes m32)
        {
            Matrizes m23 = m32.Inverse();
            return m23;
        }



        public Vector3D GetPonto3D(Vector2D ponto2d)
        {

            Vector<double> vectorDouble = Vector<double>.Build.Dense(new double[] { ponto2d.X, ponto2d.Y });
            Vector<double> vt = vectorDouble * m23.matrix;
            return new Vector3D(vt[0], vt[1], vt[2]);
         }

        public Vector2D GetPonto2D(Vector3D ponto3d)
        {

            Vector<double> vectorDouble = this.Vector3DToVectorDouble(ponto3d);
            Vector<double> vt2d = vectorDouble * m32.matrix;

            return VectorDoubleToVetor2D(vt2d, ponto3d.cor);
        }



        private Vector<double> Vector3DToVectorDouble(Vector3D v3)
        {
            return Vector<double>.Build.Dense(new double[] { v3.X, v3.Y, v3.Z });
        }

        private Vector2D VectorDoubleToVetor2D(Vector<double> v2, Color cor)
        {
            return new Vector2D(v2[0], v2[1], cor);
        }

        public class Testes : SuiteClasseTestes
        {
            public Testes() : base("testes para transfomacoes de matrizes nao quadraticas.")
            {

            }
            public void Teste1_1(AssercaoSuiteClasse assercao)
            {
                TransformacaoIsometricaNaoQuadratica transformacao = new TransformacaoIsometricaNaoQuadratica();


                Vector3D p3_inicial = new Vector3D(2, 6, 8);
                Vector2D ponto2d_final = transformacao.GetPonto2D(p3_inicial);
                Vector3D p3_final = transformacao.GetPonto3D(ponto2d_final);

                Vector3D p3_final_sem_delta = transformacao.GetPonto3D(ponto2d_final);

                System.Console.WriteLine("ponto 3d inicial: " + p3_inicial);
                System.Console.WriteLine("ponto 3d final com delta: " + p3_final);
                System.Console.WriteLine("ponto 3d final sem delta: " + p3_final_sem_delta);

                for (int x = 0; x < 5; x++)
                    for (int y = 0; y < 5; y++)
                    {
                        Vector2D ponto2D = new Vector2D(x, y, System.Drawing.Color.FromArgb(0, 0, 0, 0));
                        Vector3D ponto3d = transformacao.GetPonto3D(ponto2D);
                        System.Console.WriteLine("2D: " + ponto2D + " 3D: " + ponto3d);

                        System.Console.ReadLine();

                    }

                System.Console.ReadLine();

            }

            public void Teste1(AssercaoSuiteClasse assercao)
            {
                TransformacaoIsometricaNaoQuadratica transformacao = new TransformacaoIsometricaNaoQuadratica();

                Vector2D ponto2d_inicial = new Vector2D(4, 5, System.Drawing.Color.FromArgb(0, 0, 0, 0));
                Vector3D p3_inicial = transformacao.GetPonto3D(ponto2d_inicial);
                Vector2D ponto2d_final = transformacao.GetPonto2D(p3_inicial);
                Vector3D p3_final = transformacao.GetPonto3D(ponto2d_final);

                System.Console.WriteLine("ponto 2d inicial: " + ponto2d_inicial);
                System.Console.WriteLine("ponto 2d final: " + ponto2d_final);

                System.Console.WriteLine("ponto 3d inicial: " + p3_inicial);
                System.Console.WriteLine("ponto 3d final: " + p3_final);


                System.Console.ReadLine();




                Vector3D ponto3d_inicial = new Vector3D(2, 5, 8);
                Vector2D p2d_inicial = transformacao.GetPonto2D(ponto3d_inicial);

                System.Console.WriteLine("ponto 3d inicial: " + ponto3d_inicial.ToString());
                System.Console.WriteLine("ponto 2d inicial: " + p2d_inicial.ToString());

                System.Console.ReadLine();
            }

            public void Teste2(AssercaoSuiteClasse assercao)
            {
                Matrizes m32 = new Matrizes(3, 2);
                m32.SetElement(0, 0, 1.0);
                m32.SetElement(1, 0, 0.0);
                m32.SetElement(2, 0, 0.5);

                m32.SetElement(0, 1, 0.0);
                m32.SetElement(1, 1, 1.0);
                m32.SetElement(2, 1, 0.5);

                Matrizes m23 = m32.Inverse();

                System.Console.WriteLine("matriz original: " + m32.GetMatriz().ToString());
                System.Console.WriteLine("matriz invera:" + m23.GetMatriz().ToString());

                System.Console.WriteLine("matriz auxiliar: " + Matrizes.matrixAux1.GetMatriz().ToString());


                System.Console.ReadLine();
                assercao.IsTrue(true); // teste executado sem erros fatais.
            }


            public void Teste3(AssercaoSuiteClasse assercao)
            {
                Matrizes M32 = new Matrizes(3, 2);
                M32.SetElement(0, 0, 1.0);
                M32.SetElement(1, 0, 0.0);
                M32.SetElement(2, 0, 0.5);

                M32.SetElement(0, 1, 0.0);
                M32.SetElement(1, 1, 1.0);
                M32.SetElement(2, 1, 0.5);

                

                Matrizes vetor3_inicial = new Matrizes(1, 3);
                vetor3_inicial.SetElement(0, 0, 1);
                vetor3_inicial.SetElement(0, 1, 2);
                vetor3_inicial.SetElement(0, 2, 3);
                Matrizes vetor2_intermediario = vetor3_inicial * M32;
                Matrizes vetor3_final = vetor2_intermediario * M32.Inverse();

                Console.WriteLine("vetor 3d inicial: " + vetor3_inicial.GetMatriz());
                Console.WriteLine("vetor 3d final: " + vetor3_final.GetMatriz());
                Console.WriteLine();
                Console.WriteLine("matriz M32 inicial: " + M32.GetMatriz().ToString());
                Console.WriteLine("matriz M32 final: " + M32.GetMatriz().ToString());
                Console.WriteLine();
                Console.WriteLine();


                Console.ReadLine();

            }

            public void Teste4(AssercaoSuiteClasse assercao)
            {
                Matrizes m1 = new Matrizes(3, 3);
                Matrizes m2 = new Matrizes(3, 3);
                m1.Fill(11);
                m2.Fill(6);

                double g = Matrizes.GrauSemelhanca(m1, m2, 11);

                Console.WriteLine(m1.GetMatriz());
                Console.WriteLine(m2.GetMatriz());

                Console.WriteLine("grau: " + g);

                Console.ReadLine();

            }

        }



    }

}
