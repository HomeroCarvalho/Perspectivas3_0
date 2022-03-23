using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Testes;
using MathNet.Numerics.LinearAlgebra;

namespace ProjetoMatrizesNaoQuadraticas.bibliotecasMatematicas
{

    public class AddOnMatrix
    {
    
        public static double Determinante(Matrix<double> mt_of_determinante)
        {
            if (mt_of_determinante.RowCount != mt_of_determinante.ColumnCount)
                return 0.0;

            double det = 0.0;
            if ((mt_of_determinante.RowCount == 1) || (mt_of_determinante.ColumnCount == 1))
                return mt_of_determinante[0, 0];

            for (int col = 0; col < mt_of_determinante.ColumnCount; col++)
            {
                int signal = 0;
                if ((0 + col) % 2 == 0)
                    signal = +1;
                else
                    signal = -1;
                Matrix<double> cof = Cofactor(mt_of_determinante, 0, col);

                det += signal * mt_of_determinante[0, col] * Determinante(cof);
            }
            return det;
        }



 
        public static Matrix<double> Cofactor(Matrix<double> m, int linha, int coluna)
        {
            Matrix<double> mt_cofator = m.Clone();

            mt_cofator = mt_cofator.RemoveRow(linha);
            mt_cofator = mt_cofator.RemoveColumn(coluna);

            return mt_cofator;

        }

        public static Matrix<double> GetMatrixCofactors(Matrix<double> m_initial)
        {
            Matrix<double> mt_cofator = Matrix<double>.Build.Sparse(m_initial.RowCount, m_initial.ColumnCount);

            for (int linha = 0; linha < m_initial.RowCount; linha++)
                for (int coluna = 0; coluna < m_initial.ColumnCount; coluna++)
                    mt_cofator[linha, coluna] = Determinante(Cofactor(m_initial, linha, coluna));

            return mt_cofator;
        }

        public static Matrix<double> Transposta(Matrix<double> mt_to_transpose)
        {
            Matrix<double> mt_transposta = Matrix<double>.Build.Sparse(mt_to_transpose.ColumnCount, mt_to_transpose.RowCount);

            for (int lin = 0; lin < mt_to_transpose.RowCount; lin++)
                for (int col = 0; col < mt_to_transpose.ColumnCount; col++)
                    mt_transposta[lin, col] = mt_to_transpose[col, lin];

            return mt_transposta;
        }

        public static Matrix<double> Inversa(Matrix<double> mt_to_invert)
        {
            Matrix<double> cofactors = GetMatrixCofactors(mt_to_invert);
            Matrix<double> transpose_cofactors = Transposta(cofactors);


            double det = Determinante(mt_to_invert);



            Matrix<double> mt_inverse = (1.0 / det) * transpose_cofactors;
            return mt_inverse;

        }




        public static Matrix<double> Add3(Matrix<double> m1, Matrix<double> m2)
        {
            if ((m1.RowCount == 3) && (m1.ColumnCount == 3) && (m1.RowCount == m2.RowCount) && (m1.ColumnCount == m2.ColumnCount))
            {
                Matrix<double> mt_added = Matrix<double>.Build.Sparse(3, 3);
                for (int lin = 0; lin < 3; lin++)
                    for (int col = 0; col < 3; col++)
                        mt_added[lin, col] = m1[lin, col] + m2[lin, col];

                return mt_added;
            }
            else
                return m1 + m2;
        }

        public static Matrix<double> Sub3(Matrix<double> m1, Matrix<double> m2)
        {
            if ((m1.RowCount == 3) && (m1.ColumnCount == 3) && (m1.RowCount == m2.RowCount) && (m1.ColumnCount == m2.ColumnCount))
            {
                Matrix<double> mt_added = Matrix<double>.Build.Sparse(3, 3);
                for (int lin = 0; lin < 3; lin++)
                    for (int col = 0; col < 3; col++)
                        mt_added[lin, col] = m1[lin, col] - m2[lin, col];

                return mt_added;
            }
            else
                return m1 + m2;
        }


        public static Matrix<double> Mul(Matrix<double> m1, Matrix<double> m2)
        {
            if (m1.ColumnCount == m2.RowCount)
            {
                Matrix<double> m_mul = Matrix<double>.Build.Sparse(m1.RowCount, m2.ColumnCount);
                for (int lin = 0; lin < m1.RowCount; lin++)
                    for (int col = 0; col < m2.ColumnCount; col++)
                        for (int k = 0; k < m1.ColumnCount; k++)
                            m_mul[lin, col] += m1[lin, k] * m2[k, col];

                return m_mul;
            }
            else
                return m1 * m2;
        }


        public static Vector<double> MulVector(Vector<double> v, Matrix<double> m)
        {
            Matrix<double> mt_vector = Matrix<double>.Build.Sparse(1, v.Count);
            for (int x = 0; x < v.Count; x++)
                mt_vector[0, x] = v[x];

            Matrix<double> mt_mult = Mul(mt_vector, m);

            Vector<double> vt_result = Vector<double>.Build.Sparse(mt_mult.ColumnCount);
            for (int x = 0; x < mt_mult.ColumnCount; x++)
                vt_result[x] = mt_mult[0, x];

            return vt_result;
        }

        public class Testes : SuiteClasseTestes
        {
            public Testes() : base("testes para funcionalidades de matrizes")
            {
            }

            public void Teste_MatrizesInversas(AssercaoSuiteClasse assercao)
            {
                Matrix<double> mt_teste = Matrix<double>.Build.Random(3, 3);
                Matrix<double> mt_teste_inversa = Inversa(mt_teste);

                System.Console.WriteLine("determinante calculado:" + Determinante(mt_teste));
                System.Console.WriteLine("determinante lib: " + mt_teste.Determinant());


                IsometricaNaoQuadratica.PrintMatrix("matriz cofatores: ", GetMatrixCofactors(mt_teste));
                IsometricaNaoQuadratica.PrintMatrix("matriz cofatores transposta: ", Transposta(GetMatrixCofactors(mt_teste)));
                IsometricaNaoQuadratica.PrintMatrix("matriz cofatores transposta lib: ", GetMatrixCofactors(mt_teste).Transpose());


                IsometricaNaoQuadratica.PrintMatrix("matriz original: ", mt_teste);
                IsometricaNaoQuadratica.PrintMatrix("matriz inversa: ", mt_teste_inversa);
                IsometricaNaoQuadratica.PrintMatrix("matriz inversa lib: ", mt_teste.Inverse());

                assercao.IsTrue(Math.Abs(mt_teste_inversa[0, 0] - mt_teste.Inverse()[0, 0]) < 0.001);
                System.Console.ReadLine();
                System.Environment.Exit(1);
            }


            public void Teste_Determinante(AssercaoSuiteClasse assercao)
            {

                Matrix<double> mt_teste = Matrix<double>.Build.Random(3, 3);

                double determinateTestes = AddOnMatrix.Determinante(mt_teste);

                System.Console.WriteLine("determinante calculado: " + determinateTestes);
                System.Console.WriteLine("determinante lib: " + mt_teste.Determinant());


                System.Environment.Exit(1);
            }



            public void Teste_MultiplicacaoMatrizes(AssercaoSuiteClasse assercao)
            {
                Matrix<double> mt_mult1 = Matrix<double>.Build.Random(3, 3);
                Matrix<double> mt_mult2 = Matrix<double>.Build.Random(3, 3);
                mt_mult2[0, 0] = 2.0;
                mt_mult2[1, 1] = 1.0;
                mt_mult2[2, 2] = 1.0;

                Matrix<double> mt_result = Mul(mt_mult1, mt_mult2);
                Matrix<double> mt_resultLib = mt_mult1 * mt_mult2;

                IsometricaNaoQuadratica.PrintMatrix("matriz resultante: ", mt_result);
                IsometricaNaoQuadratica.PrintMatrix("matriz resultante lib: ", mt_resultLib);

                System.Console.ReadLine();
                System.Environment.Exit(1);
            }

            public void Teste_Cofatores(AssercaoSuiteClasse assercao)
            {
                AddOnMatrix funcionaliades = new AddOnMatrix();
                Matrix<double> mt_teste = Matrix<double>.Build.Sparse(3, 3);
                mt_teste[0, 0] = 2.0;
                mt_teste[1, 1] = 5.0;
                mt_teste[2, 2] = 7.0;

                Matrix<double> mt_cofatores = AddOnMatrix.GetMatrixCofactors(mt_teste);

                IsometricaNaoQuadratica.PrintMatrix("matriz base: ", mt_teste);
                IsometricaNaoQuadratica.PrintCofatores("cofatores:", mt_teste, 3, 3);


                System.Console.ReadLine();
                assercao.IsTrue(true); // teste feito sem erros fatais.

                System.Environment.Exit(1);
            }



        }
    }


    public class IsometricaNaoQuadratica
    {
        public Matrizes m32;
        public Matrizes m23;

        Matrix<double> k;


        public double c1 = 6.0;


        public static int precisaoDigitos = 11;

        public IsometricaNaoQuadratica()
        {
            this.m32 = this.M32();
            this.m23 = this.M23();

            k = Matrix<double>.Build.Sparse(3, 3);
            k[2, 0] = 0.0;
            k[2, 1] = 0.0;
            k[2, 2] = c1;

        }

        private Matrizes M32()
        {
            Matrizes m32 = new Matrizes(3, 2);
            m32.SetElement(0, 0, 1.0);
            m32.SetElement(1, 0, 0.0);
            m32.SetElement(2, 0, 0.5);

            m32.SetElement(0, 1, 0.0);
            m32.SetElement(1, 1, 1.0);
            m32.SetElement(2, 1, 0.5);

            return m32;
        }

        private Matrizes M23()
        {
            if (this.m32 == null)
                this.m32 = this.M32();

            return m32.Inverse(this.m32);
        }



        private Matrix<double> I3()
        {
            Matrix<double> Identidade = Matrix<double>.Build.Sparse(3, 3);
            Identidade[0, 0] = 1.0;
            Identidade[0, 1] = 0.0;
            Identidade[0, 2] = 0.0;

            Identidade[1, 0] = 0.0;
            Identidade[1, 1] = 1.0;
            Identidade[1, 2] = 0.0;

            Identidade[2, 0] = 0.0;
            Identidade[2, 1] = 0.0;
            Identidade[2, 2] = 1.0;

            return Identidade;
        }

        public Matrix<double> Matrix_Corretiva()
        {
            Matrix<double> B = Matrix<double>.Build.Sparse(3, 3);
            for (int lin = 0; lin < 3; lin++)
                for (int col = 0; col < 3; col++)
                    B[lin, col] = 0.0;


            Matrix<double> m33 = AddOnMatrix.Mul(m32.matrix, m23.matrix);

            // MATRIZ CORRETIVA DA DEPENDENCIA LINEAR.
            B[2, 2] = -m33[2, 2] + 31501711.0; // CORREÇÃO NA 3a. LINHA.


            B[1, 1] = -m33[1, 1] + 34711525.7; // CORREÇÕES PARA DETERMINANTE!=0.
            B[0, 0] = -m33[0, 0] + 15110648.6;


            /*
            B[0, 2] = -m33[0, 2] + 255067.3; // CORREÇÃO NA 3A. COLUNA, CONFORME ANALISE DE DEPENDENCIA DA MATRIZ M32*M23.
            B[1, 2] = -m33[1, 2] + 507718.0;
            */

            // k1=[(B+m32*m32)(-1)]
            // Y(-1)= k1*[(I-B*k1)(-1)]
            Matrix<double> k1 = AddOnMatrix.Inversa(AddOnMatrix.Add3(B, m33));
            Matrix<double> Y_inv = AddOnMatrix.Mul(k1, AddOnMatrix.Inversa(AddOnMatrix.Sub3(I3(), AddOnMatrix.Mul(B, k1))));




            PrintMatrix("MATRIZ B+m32*32: ", AddOnMatrix.Add3(B, m33));
            PrintMatrix("Matriz B*k1: ", AddOnMatrix.Mul(B, k1));
            PrintMatrix("Matriz B: ", B);
            PrintMatrix("Matriz k1: ", k1);
            PrintMatrix("Matriz corretiva:", Y_inv);


            PrintMatrix("[(B+m33)(-1)]: ", AddOnMatrix.Inversa(AddOnMatrix.Add3(B, m33)));





            PrintMatrix("MATRIZ (I-B*k1): ", AddOnMatrix.Inversa(AddOnMatrix.Sub3(I3(), AddOnMatrix.Mul(B, k1))));
            PrintMatrix("MATRIZ B: ", B);
            PrintMatrix("MATRIZ m32*m23: ", m33);

            PrintMatrix("MATRIZ k1: ", k1);
            PrintMatrix("MATRIZ B*k1: ", B * k1);

            System.Console.WriteLine();
            System.Console.WriteLine();
            System.Console.WriteLine("DETERMINANTES: ");

            System.Console.WriteLine("determinante  de B: " + B.Determinant());
            System.Console.WriteLine("determinante de (B+m33): " + AddOnMatrix.Determinante(AddOnMatrix.Add3(B, m33)));
            System.Console.WriteLine("determinante de (I-B*k1):  " + AddOnMatrix.Determinante(AddOnMatrix.Sub3(I3(), AddOnMatrix.Mul(B, k1))));
            System.Console.WriteLine("determinante de k1: " + AddOnMatrix.Determinante(k1));


            System.Console.WriteLine();
            System.Console.WriteLine();


            PrintMatrix("MATRIZ (I-B*k1): ", AddOnMatrix.Sub3(I3(), AddOnMatrix.Mul(B, k1)));

            return Y_inv;
        }

        public Vector<double> GetPonto3D(Vector<double> ponto2d)
        {

            Vector<double> ponto3d_inicial = ponto2d * m23.matrix;
            return ponto3d_inicial;

        }





        public Vector<double> GetPonto2D(Vector<double> p3_inicial)
        {
            return p3_inicial * m32.matrix;
        }


        public static void PrintMatrix(string caption, Matrix<double> m)
        {

            int lin = m.RowCount;
            int col = m.ColumnCount;

            string str_precisao = "N" + precisaoDigitos.ToString();

            System.Console.WriteLine(caption);
            for (int linha = 0; linha < lin; linha++)
            {
                System.Console.Write("| ");

                int coluna = 0;
                for (coluna = 0; coluna < col - 1; coluna++)
                    System.Console.Write(m[linha, coluna].ToString(str_precisao) + " , ");

                System.Console.Write(m[linha, coluna].ToString(str_precisao) + "|");

                System.Console.WriteLine();

            }
            if (m.RowCount == m.ColumnCount)
                System.Console.WriteLine("determinante: " + m.Determinant());
            System.Console.WriteLine();
            System.Console.WriteLine();
        }

        public static void PrintVector(string caption, Vector<double> v, int size)
        {
            string str_precisao = "N" + precisaoDigitos.ToString();

            int x = 0;
            System.Console.Write(caption + "  ");
            System.Console.Write("[ ");
            for (x = 0; x < size - 1; x++)
                System.Console.Write(Math.Round(v[x]).ToString(str_precisao) + ",");

            System.Console.WriteLine(Math.Round(v[x]).ToString(str_precisao) + " ]");
        }

        public static void PrintCofactor(Matrix<double> m, int linha, int coluna)
        {

            string str_precisao = "N" + precisaoDigitos;

            Matrix<double> mt_cofator = m.Clone();

            mt_cofator = mt_cofator.RemoveRow(linha - 1);
            mt_cofator = mt_cofator.RemoveColumn(coluna - 1);
            double valorConfator = mt_cofator.Determinant();

            PrintMatrix("valor: " + valorConfator.ToString(str_precisao) + "   cofator [" + linha + "," + coluna + "] :", mt_cofator);
            System.Console.WriteLine();
            System.Console.WriteLine();

        }


        public static void PrintCofatores(string caption, Matrix<double> mt, int line, int col)
        {
            PrintMatrix(caption, mt);

            for (int linha = 1; linha <= line; linha++)
                for (int coluna = 1; coluna <= col; coluna++)
                    PrintCofactor(mt, linha, coluna);

        }


        public static void Fill(ref Matrix<double> mt, double fatorIndice, double step)
        {
            for (int linha = 0; linha < mt.RowCount; linha++)
                for (int coluna = 0; coluna < mt.ColumnCount; coluna++)
                {
                    mt[linha, coluna] = fatorIndice;
                    fatorIndice += step;
                }
        }



        public class Testes : SuiteClasseTestes
        {

            public Testes() : base("teste para transformacao com matrizes nao quadraticas.")
            {

            }

            public void TesteValidadcao_07(AssercaoSuiteClasse assercao)
            {


                IsometricaNaoQuadratica naoQuadratica = new IsometricaNaoQuadratica();
                Matrix<double> m32 = naoQuadratica.M32().matrix;
                Matrix<double> m23 = naoQuadratica.M23().matrix;


                Matrix<double> Y_inv = naoQuadratica.Matrix_Corretiva();


                IsometricaNaoQuadratica.PrintMatrix("corretiva: (Y(-1)): ", Y_inv);
                IsometricaNaoQuadratica.PrintMatrix("matriz Y_inv: ", Y_inv);
                IsometricaNaoQuadratica.PrintMatrix("identidade Y: ", AddOnMatrix.Mul(Y_inv, AddOnMatrix.Inversa(Y_inv)));



                System.Console.WriteLine();
                System.Console.WriteLine();
                System.Console.WriteLine();

                Vector<double> v3_inital = Vector<double>.Build.Sparse(3);

                v3_inital[0] = 1.0;
                v3_inital[1] = 3.0;
                v3_inital[2] = 6.0;


                Vector<double> v2_comCorretiva = AddOnMatrix.MulVector(v3_inital, AddOnMatrix.Mul(Y_inv, m32));
                Vector<double> v2_semCorretiva = AddOnMatrix.MulVector(v3_inital, m32);

                Vector<double> v3_COM_CORRETIVA = AddOnMatrix.MulVector(v2_comCorretiva, m23);
                Vector<double> v3_SEM_CORRETIVA = AddOnMatrix.MulVector(v2_semCorretiva, m23);



                System.Console.WriteLine("VETORES 2D GERADOS: ");
                System.Console.WriteLine("v2 corretiva: " + v2_comCorretiva);
                System.Console.WriteLine("v2 nao corretiva: " + v2_semCorretiva);

                System.Console.WriteLine();
                System.Console.WriteLine();

                System.Console.WriteLine("VETORES 3D GERADOS: ");

                IsometricaNaoQuadratica.PrintVector("v3 inicial: ", v3_inital, 3);
                IsometricaNaoQuadratica.PrintVector("v3 COM corretiva: ", v3_COM_CORRETIVA, 3);
                IsometricaNaoQuadratica.PrintVector("v3 SEM corretiva: ", v3_SEM_CORRETIVA, 3);

                System.Console.WriteLine();
                System.Console.WriteLine();


                System.Console.ReadLine();
                System.Environment.Exit(1);
            }


            private Matrix<double> ValidacaoIdentidade(Matrix<double> m32, Matrix<double> m23, Matrix<double> X)
            {
                Matrix<double> m32xm23 = m32 * m23;
                EscalonamentoMatriz escalonamento = new EscalonamentoMatriz(m32xm23);
                escalonamento.ScaleRows();
                escalonamento.ScaleColumns();


                return escalonamento.mt_scaled;
            }

            public void TesteIdentidade(AssercaoSuiteClasse assercao)
            {
                IsometricaNaoQuadratica transformacao = new IsometricaNaoQuadratica();
                System.Console.WriteLine("I3: " + transformacao.M32().matrix * transformacao.M23().matrix);
                System.Console.WriteLine("I2: " + transformacao.M23().matrix * transformacao.M32().matrix);

                System.Console.ReadLine();
            }


            public void Teste_6(AssercaoSuiteClasse assercao)
            {
                IsometricaNaoQuadratica transformacao = new IsometricaNaoQuadratica();

                Matrizes M33 = new Matrizes(3, 3);
                M33.matrix = transformacao.m32.matrix * transformacao.m23.matrix;


                System.Console.WriteLine("Matriz M33: " + M33.matrix);

                M33.matrix[2, 0] = 5.0;
                M33.matrix[2, 1] = 7.0;
                M33.matrix[2, 2] = 15.0;

                Matrizes M33_inv_1 = new Matrizes(3, 3);

                M33_inv_1.matrix = M33.matrix.Inverse();

                System.Console.WriteLine("M33 inversa 1: " + M33_inv_1.matrix);
                System.Console.WriteLine("Matriz Identidade: " + M33.matrix * M33_inv_1.matrix);

                System.Console.WriteLine();
                System.Console.WriteLine();
                System.Console.WriteLine();

                M33 = transformacao.m32 * transformacao.m23;

                M33.matrix[2, 0] = 3.0;
                M33.matrix[2, 1] = 5.0;
                M33.matrix[2, 2] = 8.0;

                Matrizes M33_inv_2 = new Matrizes(3, 3);



                M33_inv_2.matrix = M33.matrix.Inverse();

                System.Console.WriteLine("M33 inversa 2: " + M33_inv_2.matrix);
                System.Console.WriteLine("Matriz Identidade: " + M33.matrix * M33_inv_2.matrix);


                Vector<double> v_teste_01_inicial = Vector<double>.Build.Dense(new double[] { 3, 8, 15 });
                Vector<double> v_teste_01_final = v_teste_01_inicial * M33.matrix * M33_inv_2.matrix;

                System.Console.WriteLine("vetor inicial: " + v_teste_01_inicial);
                System.Console.WriteLine("vetor   final: " + v_teste_01_final);

                System.Console.ReadLine();

            }


            public void Teste4(AssercaoSuiteClasse assercao)
            {
                Matrizes m1 = new Matrizes(3, 3);
                Matrizes m2 = new Matrizes(3, 3);
                Matrizes.Fill(ref m1, 4, 5);
                Matrizes.Fill(ref m2, 4, 7);

                double g = Matrizes.GrauSemelhanca(m1, m2, 11);

                Console.WriteLine(m1.GetMatriz());
                Console.WriteLine(m2.GetMatriz());

                Console.WriteLine("grau: " + g);

                Console.ReadLine();

            }

        }



    }

}
