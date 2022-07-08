using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Testes;
using MathNet.Numerics.LinearAlgebra;

namespace ProjetoMatrizesNaoQuadraticas.bibliotecasMatematicas
{
    public class Matriz
    {
        public double[,] data
        { 
            get;
            set;
        }
        
        
        public int lines
        { 
            get;
            private set;
        }
        public int columns
        { 
            get;
            private set;
        }



        public static bool isDebug = false;
        public Matriz(int line, int column)
        {
            this.lines = line;
            this.columns = column;
            this.data = new double[line, column];
            for (int lin = 0; lin < this.lines; lin++)
                for (int col = 0; col < this.columns; col++)
                    this.data[lin, col] = 0.0;
        }



        public Matriz Clone()
        {
            Matriz mt_cloned = new Matriz(this.lines, this.columns);
            for (int lin = 0; lin < this.lines; lin++)
                for (int col = 0; col < this.columns; col++)
                    mt_cloned.data[lin, col] = this.data[lin, col];

            return mt_cloned;
        }
        



        public static Matriz Inversa(Matriz mt_to_invert)
        {

            Matrix<double> mt_double = MatrizToMatrixDouble(mt_to_invert);
            Matrix<double> mt_double_inversa = mt_double.Inverse();
            return MatrixDoubleToMatriz(mt_double_inversa);

        }

        public  Matriz Inversa()
        {
            Matrix<double> mt_double = MatrizToMatrixDouble(this);
            Matrix<double> mt_double_inversa = mt_double.Inverse();

            return MatrixDoubleToMatriz(mt_double_inversa);
        }


        public static Matriz Add(Matriz m1, Matriz m2)
        {

            Matrix<double> mt1 = MatrizToMatrixDouble(m1);
            Matrix<double> mt2 = MatrizToMatrixDouble(m2);

            Matrix<double> mt_result = mt1 + mt2;

            return MatrixDoubleToMatriz(mt_result);



        }


        public static Matriz Sub(Matriz m1, Matriz m2)
        {
            Matrix<double> mt1 = MatrizToMatrixDouble(m1);
            Matrix<double> mt2 = MatrizToMatrixDouble(m2);

            Matrix<double> mt_result = mt1 - mt2;

            return MatrixDoubleToMatriz(mt_result);

        }


        public static Matriz Mul(Matriz m1, Matriz m2)
        {

            Matrix<double> mt1 = MatrizToMatrixDouble(m1);
            Matrix<double> mt2 = MatrizToMatrixDouble(m2);

            Matrix<double> mt_result = mt1 * mt2;

            return MatrixDoubleToMatriz(mt_result);
        }

        public static Matriz Mul(double number,Matriz m1)
        {
            Matrix<double> mt1 = MatrizToMatrixDouble(m1);
            Matrix<double> mt_result = number * mt1;

            return MatrixDoubleToMatriz(mt_result);
        }

        public MyVetor ToVetor()
        {
            MyVetor vetor = new MyVetor(this.columns);

            for (int x = 0; x < this.columns; x++)
                vetor.data[x] = this.data[0, x];

            return vetor;
        }


        private static List<List<double>> ToListMatrix(Matriz mt_current)
        {
            if (mt_current.data == null)
                return null;
            else
            {
                List<List<double>> lst_matriz = new List<List<double>>();
                List<double> umaLinha = new List<double>();
                for (int lin = 0; lin < mt_current.lines; lin++)
                {
                    umaLinha.Clear();
                    for (int col = 0; col < mt_current.columns; col++)
                        umaLinha.Add(mt_current.data[lin, col]);
                    lst_matriz.Add(umaLinha.ToList<double>());

                }

                return lst_matriz;
            }
        }

        private static Matriz ToArrayMatrix(List<List<double>> lst_matrix)
        {
            if ((lst_matrix == null) || (lst_matrix.Count == 0))
                return null;
            else
            {
                Matriz mt_return = new Matriz(lst_matrix.Count, lst_matrix[0].Count);
                for (int lin = 0; lin < lst_matrix.Count; lin++)
                    for (int col = 0; col < lst_matrix[0].Count; col++)
                        mt_return.data[lin, col] = lst_matrix[lin][col];

                return mt_return;
            }
        }

        public void PrintMatrix(string caption)
        {
            PrintMatrix(caption, this);
        }

        public static  void PrintMatrix(string caption, Matriz mt_to_print)
        {

            int lin = mt_to_print.lines;
            int col = mt_to_print.columns;

            string str_precisao = "N" + MyTransformacaoIsometrica.precisaoDigitos.ToString();

            System.Console.WriteLine(caption);
            for (int linha = 0; linha < lin; linha++)
            {
                System.Console.Write("| ");

                int coluna = 0;
                for (coluna = 0; coluna < col - 1; coluna++)
                    if (!isDebug)
                        System.Console.Write(mt_to_print.data[linha, coluna].ToString(str_precisao) + " , ");
                    else
                        System.Console.Write(Math.Round(mt_to_print.data[linha, coluna]) + " , ");


                if (!isDebug)
                    System.Console.Write(mt_to_print.data[linha, coluna].ToString(str_precisao) + "|");
                else
                    System.Console.Write(Math.Round(mt_to_print.data[linha, coluna]) + "|");

                System.Console.WriteLine();

            }

           System.Console.WriteLine();
            System.Console.WriteLine();
        }

        public override string ToString()
        {
           
            string str = "[";
            for (int lin = 0; lin < this.lines; lin++)
            {
                str += "[";
                
                
                int col = 0;
                for (col = 0; col < this.columns - 1; col++)
                    str += this.data[lin, col].ToString("N2")+",";

                str += this.data[lin, col].ToString("N2");

                str += "]";
            }
            str += "]";
            return str;
        }



        public Matriz Fill(int fatorIndice)
        {
            Matriz mt_filled = new Matriz(this.lines, this.columns);
            for (int linha = 0; linha < this.lines; linha++)
                for (int coluna = 0; coluna < this.columns; coluna++)
                {
                    mt_filled.data[linha, coluna] = fatorIndice;
                    fatorIndice += 4;
                }

            return mt_filled;
        }


        public void Fill(int fatorIndice, ref Matrix<double> mt_filled)
        {
            for (int linha = 0; linha < mt_filled.RowCount; linha++)
                for (int coluna = 0; coluna < mt_filled.ColumnCount; coluna++)
                {
                    mt_filled[linha, coluna] = fatorIndice;
                    fatorIndice += 4;
                }
        }


        /// <summary>
        /// Calcula a Matriz Inversa nao quadrática, se a matriz que chamou este metodo
        /// tenha numero de linhas <> numero de colunas.
        /// Se tiver numero de linhas==numero de colunas, calcula a inversa quadrática.
        /// </summary>
        public Matriz InversaNaoQuadratica()
        {
            if (this.lines > this.columns)
            {
                /// exemplo do calculo da matriz nao quadratica, com utilizacao de matriz auxliar.
                /// M[3,2]*M[2,3]=I(3), M[3,2] é conhecida.
                /// Aux1[2,3]*M[3,2]*M[2,3]=Aux1[2,3]*I(3),
                /// Aux1M[2,2]*M[2,3]=Aux1[2,3]
                /// Aux1M32(-1)*Aux1M[2,2]*M[2,3]=Aux1M32(-1)*Aux1[2,3]
                /// M[2,3]=Aux1M32(-1)*Aux1[2,3]
                /// 
                Matrix<double> aux23 = Matrix<double>.Build.Sparse(2, 3);
                Fill(4, ref aux23);


                Matrix<double> M32 = MatrizToMatrixDouble(this);
                Matrix<double> MAuxM32 = aux23 * M32;
                Matrix<double> MAuxM32Inv = MAuxM32.Inverse();
                Matrix<double> MAuxM_Aux23 = MAuxM32Inv * aux23;

                Matriz M23 = MatrixDoubleToMatriz(MAuxM_Aux23);

                return M23;


            }
            else
            if (this.lines < this.columns)
            {
                /// exemplo do calculo da matriz nao quadratica, com utilizacao de matriz auxliar.
                /// M[3,2]*M[2,3]=I(3); M[2,3] é conhecida.
                /// M[3,2]*M[2,3]*Aux1[3,2]=I(3)*Aux1[3,2]
                /// M[3,2]*MAux1[2,2]=Aux1[3,2]
                /// M[3,2]*MAux1[2,2]*MAux1[2,2](-1)=Aux1[3,2]*MAux1[2,2](-1)
                /// M[3,2]=Aux1[3,2]*MAux1[2,2](-1)

                Matrix<double> aux32 = Matrix<double>.Build.Sparse(3, 2);
                Fill(4, ref aux32);

                Matrix<double> M23 = MatrizToMatrixDouble(this);
                Matrix<double> M23Aux32 = M23 * aux32;

                Matrix<double> M23Aux32_inv = (M23Aux32).Inverse();
                Matrix<double> M23Aux32_Aux32 = M23Aux32_inv * aux32;

                Matriz M32 = MatrixDoubleToMatriz(M23Aux32_Aux32);

                return M32;
            }
            else
                return null;
        }


        public static Matrix<double> MatrizToMatrixDouble(Matriz mt)
        {
            Matrix<double> mt_result = Matrix<double>.Build.Sparse(mt.lines, mt.columns);
            for (int lin = 0; lin < mt.lines; lin++)
                for (int col = 0; col < mt.columns; col++)
                    mt_result[lin, col] = mt.data[lin, col];
            return mt_result;
        }

        public static Matriz MatrixDoubleToMatriz(Matrix<double> mt)
        {
            Matriz mt_result = new Matriz(mt.RowCount, mt.ColumnCount);
            for (int lin = 0; lin < mt.RowCount; lin++)
                for (int col = 0; col < mt.ColumnCount; col++)
                    mt_result.data[lin, col] = mt[lin, col];

            return mt_result;
        }



        public class Testes : SuiteClasseTestes
        {
           
            public Testes() : base("testes para funcionalidades de matrizes")
            {
            }


            public void Teste_MatrizInversa(AssercaoSuiteClasse assercao)
            {
                Matriz.isDebug = false;

                Matriz mt_teste = new Matriz(3, 2);
                mt_teste.data[0, 0] = 1.0;
                mt_teste.data[1, 0] = 0.0;
                mt_teste.data[2, 0] = 0.5;

                mt_teste.data[0, 1] = 0.0;
                mt_teste.data[1, 1] = 1.0;
                mt_teste.data[2, 1] = 0.5;



                Matriz mt_teste_inversa = mt_teste.InversaNaoQuadratica();

                Matriz.PrintMatrix("matriz teste: ", mt_teste);
                Matriz.PrintMatrix("matriz inversa: ", mt_teste_inversa);
                Matriz.PrintMatrix("matriz identidade: ", Matriz.Mul(mt_teste, mt_teste_inversa));


                System.Console.ReadLine();

            }

            public void Teste_MultiplicacaoMatrizes(AssercaoSuiteClasse assercao)
            {
                System.Console.WriteLine("testes comparativos para matrizes inversas.");
                Matrix<double> mt_mult1 = Matrix<double>.Build.Random(3, 3);
                Matrix<double> mt_mult2 = Matrix<double>.Build.Random(3, 3);
         


                Matriz mt_mulMatriz1 = MatrixDoubleToMatriz(mt_mult1);
                Matriz mt_mulMatriz2 = MatrixDoubleToMatriz(mt_mult2);

                Matriz mt_result = Mul(mt_mulMatriz1, mt_mulMatriz2);
                Matrix<double> mt_resultLib = mt_mult1 * mt_mult2;

                PrintMatrix("matriz resultante: ", mt_result);

                System.Console.WriteLine("matriz resultante lib: ", mt_resultLib);

                System.Console.ReadLine();
            }

            public void Teste_AddMatriz(AssercaoSuiteClasse assercao)
            {
                System.Console.WriteLine("Adicao de matrizes");

                Matrix<double> mt_lib_add1 = Matrix<double>.Build.Sparse(3, 3);
                Matrix<double> mt_lib_add2 = Matrix<double>.Build.Sparse(3, 3);
                mt_lib_add2[0, 0] = 2.0;
                mt_lib_add2[1, 1] = 2.0;
                mt_lib_add2[2, 2] = 2.0;


                mt_lib_add1[0, 0] = 3.0;
                mt_lib_add1[1, 1] = 3.0;
                mt_lib_add1[2, 2] = 3.0;

                Matriz mt_add1 = MatrixDoubleToMatriz(mt_lib_add1);
                Matriz mt_add2 = MatrixDoubleToMatriz(mt_lib_add2);

                Matriz mt_added = Matriz.Add(mt_add1, mt_add2);
                Matrix<double> mt_added_lib = mt_lib_add1 + mt_lib_add2;

                Matriz.PrintMatrix("matriz resultante: ", mt_added);
                System.Console.WriteLine("matriz resultante lib: ", +mt_added_lib);

                    
                assercao.IsTrue(Math.Abs(mt_added.data[0, 0] - 4) < 0.01);
                System.Console.ReadLine();



            }

            public void Teste_SubMatriz(AssercaoSuiteClasse assercao)
            {
                System.Console.WriteLine("Teste de subtração de matrizes.");

                Matrix<double> mt_lib_sub1 = Matrix<double>.Build.Sparse(3, 3);
                Matrix<double> mt_lib_sub2 = Matrix<double>.Build.Sparse(3, 3);
                mt_lib_sub2[0, 0] = 2.0;
                mt_lib_sub2[1, 1] = 2.0;
                mt_lib_sub2[2, 2] = 2.0;


                mt_lib_sub1[0, 0] = 3.0;
                mt_lib_sub1[1, 1] = 3.0;
                mt_lib_sub1[2, 2] = 3.0;

                Matriz mt_sub_1 = MatrixDoubleToMatriz(mt_lib_sub1);
                Matriz mt_sub_2 = MatrixDoubleToMatriz(mt_lib_sub2);

                Matriz mt_sub_result = Matriz.Sub(mt_sub_1, mt_sub_2);
                Matrix<double> mt_sub_result_lib = mt_lib_sub1 - mt_lib_sub2;

                Matriz.PrintMatrix("matriz resultante: ", mt_sub_result);
                System.Console.WriteLine("matriz resultante lib: " + mt_sub_result_lib);


                assercao.IsTrue(Math.Abs(mt_sub_result.data[0, 0] - 4) < 0.01);

                System.Console.ReadLine();



            }


 

            private void GetRandomMatriz(ref Matriz mt, int seed)
            {
                Random aleatorizador = new Random(seed);
                for (int lin = 0; lin < mt.lines; lin++)
                    for (int col = 0; col < mt.columns; col++)
                        mt.data[lin, col] = aleatorizador.Next(1, 25);
            }

            private Matrix<double> MatrizToMatrixDouble(Matriz mt)
            {
                Matrix<double> mt_result = Matrix<double>.Build.Sparse(mt.lines, mt.columns);
                for (int lin = 0; lin < mt.lines; lin++)
                    for (int col = 0; col < mt.columns; col++)
                        mt_result[lin, col] = mt.data[lin, col];
                return mt_result;
            }

            private Matriz MatrixDoubleToMatriz(Matrix<double> mt)
            {
                Matriz mt_result = new Matriz(mt.RowCount, mt.ColumnCount);
                for (int lin = 0; lin < mt.RowCount; lin++)
                    for (int col = 0; col < mt.ColumnCount; col++)
                        mt_result.data[lin, col] = mt[lin, col];

                return mt_result;
            }


        }
    }

}
