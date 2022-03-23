using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Testes;
using MathNet.Numerics.LinearAlgebra;

namespace ProjetoMatrizesNaoQuadraticas.bibliotecasMatematicas
{
    /// <summary>
    /// classe que gera matriz m23,m32, e corretiva, para converter pontos 2d em pontos 3d, e vice
    /// versa.
    /// </summary>
    public partial class MyTransformacaoIsometrica
    {


        public Matriz m32 { get; set; }
        public Matriz m23 { get; set; }

        public Matriz m33;

        public static int precisaoDigitos = 21;

        public static bool isDebug = false;

        public Matriz B { get; private set; }

        private static Matriz _mtCorretiva = null;

        public Matriz mt32_corretiva
        {
            get
            {
                if (_mtCorretiva == null)
                    _mtCorretiva = Matriz_Corretiva_m32();
                return _mtCorretiva;
            }
        }

        
        public MyTransformacaoIsometrica()
        {
            this.m32 = M32();
            this.m23 = M23();
            m33 = Matriz.Mul(m32, m23);

            this.B = this.CalculoMatrizB(Matriz.Mul(m32, m23));
        }
        public Matriz M32()
        {
            Matriz mt_32 = new Matriz(3, 2);
            mt_32.data[0, 0] = 1.0;
            mt_32.data[1, 0] = 0.0;
            mt_32.data[2, 0] = 1.0 /2.0;

            mt_32.data[0, 1] = 0.0;
            mt_32.data[1, 1] = 1.0;
            mt_32.data[2, 1] = 1.0 / 2.0;

            return mt_32;
        }

        public Matriz M23()
        {
            if (this.m32 == null)
                this.m32 = this.M32();


            Matriz mt_23 = this.m32.InversaNaoQuadratica();
            return mt_23;
        }

        private Matriz CalculoMatrizB(Matriz M33)
        {
            B = new Matriz(3, 3);
            B.data[0, 0] = -M33.data[0, 0] + 5.0;
            B.data[1, 1] = -M33.data[1, 1] + 5.0;
            B.data[2, 2] = -M33.data[2, 2] + 5.0;


            B.data[0, 1] = -M33.data[0, 1];
            B.data[0, 2] = -M33.data[0, 2];


            B.data[1, 0] = -M33.data[1, 0];
            B.data[1, 2] = -M33.data[1, 2];

            
            B.data[2, 0] = -M33.data[2, 0];
            B.data[2, 1] = -M33.data[2, 1]; 
            
            return B;
        }


        public MyVetor GetPonto2D(MyVetor vetor3d)
        {
            return MyVetor.Mul(vetor3d, Matriz.Mul(this.mt32_corretiva, this.m32));
        }


        public MyVetor GetPonto3D(MyVetor vetor2d)
        {

            Matriz B_tmp = this.B.Clone();
            this.B = this.CalculoMatrizB(this.m33);


            // vetor 3d iterativo, com aproximação baseada no vetor 2d de partida.
            // é conhecido: ponto2d, B, m32,m23.

            // a equação é pnt_fini=pnt_ini(m32*m23+B).
            // mas como (m32*m23+B)=I, pela construção de B,
            // e por não termos pnt_ini, aproximamos para pnt_2d*m23.
            // a condição de parada é (pnt_iterativo-pnt_previo)=0.

            MyVetor mt_ponto3d_iterativo = MyVetor.Mul(vetor2d, m23);
            MyVetor mt_ponto3d_previo = new MyVetor(1, 3);
            double signal = B.Determinante();
            if (signal < 0.0)
                signal = -1.0;
            else
                signal = +1.0;
            int x = 0;
            for (x = 0; x < 15; x++)
            {
                MyVetor delta = Delta(mt_ponto3d_iterativo, B, 0.29, signal);

                mt_ponto3d_previo = mt_ponto3d_iterativo.Clone();
                mt_ponto3d_iterativo = MyVetor.Add(mt_ponto3d_iterativo, delta);

                if (MathVectors(mt_ponto3d_iterativo, mt_ponto3d_previo, 0.5))
                {
                    this.B = B_tmp.Clone();
                    return mt_ponto3d_iterativo;
                }

            }

            this.B = B_tmp.Clone();
            return MyVetor.Mul(vetor2d, m23);

        }

        private MyVetor Delta(MyVetor mt_vetor, Matriz mt_B, double coeficienteDeAprendizado, double signalDeterminant)
        {
            return MyVetor.Mul(mt_vetor, mt_B).Normaliza().Mul(coeficienteDeAprendizado * signalDeterminant);

        }


        private bool MathVectors(MyVetor m1, MyVetor m2, double erroAceitavel)
        {
            if (m1.lenght() != m2.lenght())
                return false;
            else
            {
                for (int lin = 0; lin < m1.lenght(); lin++)
                    if (Math.Abs(Math.Abs(m1.data[lin]) - Math.Abs(m2.data[lin])) > erroAceitavel)
                        return false;

                return true;
            }
        }
        private Matriz Get_B_fromM32xM23()
        {
            MyTransformacaoIsometrica myTransformacao = new MyTransformacaoIsometrica();
            Matriz m32xm23 = Matriz.Mul(myTransformacao.m32, myTransformacao.m23);

            Matriz B = new Matriz(m32.lines, m23.columns);
            for (int lin = 0; lin < m32.lines; lin++)
                for (int col = 0; col < m23.columns; col++)
                    B.data[lin, col] -= m32xm23.data[lin, col];

            return B;
        }


        public void Fill(ref Matriz mt, double fatorIndice,double step)
        {
            Random aleatorizador = new Random(535);
            for (int linha = 0; linha <mt.lines; linha++)
                for (int coluna = 0; coluna < mt.columns; coluna++)
                {
                    mt.data[linha, coluna] = aleatorizador.Next((int)fatorIndice) + 1;
                    fatorIndice += step;
                }
        }

        private Matriz I3()
        {
            Matriz Identidade = new Matriz(3, 3);
            Identidade.data[0, 0] = 1.0;
            Identidade.data[0, 1] = 0.0;
            Identidade.data[0, 2] = 0.0;

            Identidade.data[1, 0] = 0.0;
            Identidade.data[1, 1] = 1.0;
            Identidade.data[1, 2] = 0.0;

            Identidade.data[2, 0] = 0.0;
            Identidade.data[2, 1] = 0.0;
            Identidade.data[2, 2] = 1.0;

            return Identidade;
        }




        public Matriz Matriz_Corretiva_m23()
        {
            /// utilização:
            /// correçãp para matriz de transformação m23,
            /// com p2*M23*Y_inv
            /// 
            /// metodo: 
            ///         vetor3d GetPonto3d(ponto2d){return ponto2d*m23*Y_inv}



            double factorMult = 3.5;
                           


            Matriz B_tmp = this.B.Clone();

            this.B = new Matriz(3, 3);


            // CORREÇÃO NA 3a. LINHA.
            B.data[2, 0] += 15 * factorMult * 1.0;
            B.data[2, 1] += 27 * factorMult * 2.1;
            B.data[2, 2] += 19 * factorMult * 3.7;

            // para determinante <> 0.
            B.data[1, 1] += 10 * factorMult * 4.9;
            B.data[0, 0] += 11 * factorMult * 6.5;


            Matriz m33 = Matriz.Mul(this.m32, this.m23);
            /*
             * Y(-1)=[(I-k*B)(-1)]*k,
               com k=[(m33+B)(-1)]
             */


            Matriz k = Matriz.Inversa(Matriz.Add(m33, B));
            Matriz Y_inv = Matriz.Mul(Matriz.Inversa(Matriz.Sub(I3(), Matriz.Mul(k, B))), k);

            this.B = B_tmp.Clone();

            return Y_inv;
        }

        public  Matriz Matriz_Corretiva_m32()
        {
            double factorMult = 150000000000.00;


            // k1=[(B+m32*m32)(-1)]
            // Y(-1)= k1*[(I-B*k1)(-1)]
            Matriz B_tmp = this.B.Clone();


            this.B = new Matriz(3, 3);


            // CORREÇÃO NA 3a. LINHA.
            B.data[2, 0] += 15 * factorMult * 1.0;
            B.data[2, 1] += 27 * factorMult * 2.1;
            B.data[2, 2] += 15 * factorMult * 3.7;

            // para determinante <> 0.
            B.data[1, 1] += 10 * factorMult * 4.9;
            B.data[0, 0] += 11 * factorMult * 6.5;

            // para precisão na coordenada Y:
            // B.data[2,1]=0.0;


            // k1=[(B+m32*m32)(-1)]
            // Y(-1)= k1*[(I-B*k1)(-1)]

            Matriz k1 = Matriz.Add(B, this.m33).Inversa();
            Matriz Y_inv = Matriz.Mul(k1, Matriz.Inversa(Matriz.Sub(I3(), Matriz.Mul(B, k1))));



            this.B = B_tmp.Clone();

            return Y_inv;
        }

        public Matriz Matriz_32_corrigida(double fatorIdentidade)
        {
            Matriz m32xM23 = this.m33.Clone();

            B = new Matriz(3, 3);
            for (int lin = 0; lin < 3; lin++)
                for (int col = 0; col < 3; col++)
                    B.data[lin, col] = -m32xM23.data[lin, col];

            B.data[0, 0] += fatorIdentidade;
            B.data[1, 1] += fatorIdentidade;
            B.data[2, 2] += fatorIdentidade;

            /// k1=[(B+m32*m32)(-1)]
            /// Y(-1)= k1*[(I-B*k1)(-1)]
            /// se k1=2*I,
            /// Y(-1)=I*(I-B*2*I)(-1)
            /// Y(-1)=2*((I-2*B)(-1))
            Matriz k = Matriz.Inversa(Matriz.Add(B, m32xM23));
            Matriz Y_inv = Matriz.Mul(fatorIdentidade, Matriz.Inversa(Matriz.Sub(this.I3(), Matriz.Mul(fatorIdentidade, B))));

            return Y_inv;
        }

        public Matriz Matriz_23_corrigida(double fatorIdentidade)
        {
            
            MyTransformacaoIsometrica transformacao = new MyTransformacaoIsometrica();

            MyVetor.isDebug = true;
            MyTransformacaoIsometrica.isDebug = true;

            Matriz m32xM23 = transformacao.m33.Clone();

            B = new Matriz(3, 3);
            for (int lin = 0; lin < 3; lin++)
                for (int col = 0; col < 3; col++)
                    B.data[lin, col] = -m32xM23.data[lin, col];

            B.data[0, 0] += fatorIdentidade;
            B.data[1, 1] += fatorIdentidade;
            B.data[2, 2] += fatorIdentidade;


            /// Y(-1)=[(I-k*B)(-1)]*k,
            /// com k=[(m33+B)(-1)]
            /// Se k=2*I,
            /// Y(-1)=(I-2*B)(-1)*2,
            /// Y(-1)=2*(I-2*B)(-1).

            Matriz k = Matriz.Inversa(Matriz.Add(m32xM23, B));
            Matriz Y_inv = Matriz.Mul(fatorIdentidade, Matriz.Inversa(Matriz.Sub(transformacao.I3(), Matriz.Mul(fatorIdentidade, B))));

            return Y_inv;
        }
        public class Testes : SuiteClasseTestes
        {

            public Testes() : base("teste para transformacao com matrizes nao quadraticas.")
            {

            }

            public void Teste_09(AssercaoSuiteClasse assercao)
            {
                MyTransformacaoIsometrica transformacao = new MyTransformacaoIsometrica();

                MyVetor.isDebug = true;


                double fatorIdentidade = 0.9925;
                for (int x = 0; x < 15; x++)
                {
                    Matriz mt_corretiva32_33 = transformacao.Matriz_32_corrigida(fatorIdentidade);
                    Matriz mt_corretiva23_33 = transformacao.Matriz_23_corrigida(fatorIdentidade);

                    Matriz m32 = transformacao.m32;
                    Matriz m23 = transformacao.m23;


                    MyVetor vetor3d_inicio = new MyVetor(1, 3, 6);
                    MyVetor vt2d_COM_correcao = MyVetor.Mul(vetor3d_inicio, Matriz.Mul(mt_corretiva32_33, m32));
                    MyVetor vt2d_SEM_correcao = MyVetor.Mul(vetor3d_inicio, m32);

                    MyVetor vetor3d_fim_comCorrecao = MyVetor.Mul(vt2d_COM_correcao, Matriz.Mul(m23, mt_corretiva23_33));
                    MyVetor vetor3d_fim_SEMcorrecao = MyVetor.Mul(vt2d_SEM_correcao, m23);

                    vetor3d_inicio.PrintVector("vetor 3d inicio: ");
                    vetor3d_fim_comCorrecao.PrintVector("vetor 3d fim COM correcao: ");
                    vetor3d_fim_SEMcorrecao.PrintVector("vetor 3d fim SEM correcao: ");

                    System.Console.WriteLine();
                    System.Console.WriteLine();

                    vt2d_COM_correcao.PrintVector("vetor 2d com correcao: ");
                    vt2d_SEM_correcao.PrintVector("vetor 2d sem correcao: ");

                    System.Console.WriteLine("fator identidade:" + fatorIdentidade.ToString());

                    System.Console.WriteLine("erro: "+imprecisao(vetor3d_inicio, vetor3d_fim_comCorrecao));

                    System.Console.ReadLine();

                    fatorIdentidade *= 0.5;
                }
                System.Environment.Exit(1);

            }
            public void TesteGetPonto3D(AssercaoSuiteClasse assercao)
            {
                MyTransformacaoIsometrica transformacao = new MyTransformacaoIsometrica();
                MyTransformacaoIsometrica.isDebug = false;
                MyVetor.isDebug = true;



                MyVetor vetor3d_initial = new MyVetor(1, 3, 6);

                MyVetor vetor2d = MyVetor.Mul(vetor3d_initial, Matriz.Mul(transformacao.Matriz_Corretiva_m32(), transformacao.m32));



                MyVetor vetor3d_final = MyVetor.Mul(vetor2d, transformacao.m23);



                vetor3d_initial.PrintVector("vetor 3d initial: ");
                vetor3d_final.PrintVector("vetor 3d final: ");


                System.Console.ReadLine();
                System.Environment.Exit(1);
            }


            
            public void Teste_08(AssercaoSuiteClasse assercao)
            {
                MyTransformacaoIsometrica transformacao = new MyTransformacaoIsometrica();

                Matriz Y_inv32 = transformacao.Matriz_32_corrigida(2.0);
                Matriz Y_inv23 = transformacao.Matriz_23_corrigida(2.0);

                Y_inv23.PrintMatrix("matriz Y(2,3): ");
                Y_inv32.PrintMatrix("matriz Y(3,2): ");

                System.Console.ReadLine();
                System.Environment.Exit(1);
            }

            public void Teste_06(AssercaoSuiteClasse assercao)
            {
                MyTransformacaoIsometrica transformacao = new MyTransformacaoIsometrica();
                Matriz m32xM23 = transformacao.m33.Clone();

                Matriz B = new Matriz(3, 3);
                for (int lin = 0; lin < 3; lin++)
                    for (int col = 0; col < 3; col++)
                        B.data[lin, col] = -m32xM23.data[lin, col];

                B.data[0, 0] += 2.0;
                B.data[1, 1] += 2.0;
                B.data[2, 2] += 2.0;

                /// k1=[(B+m32*m32)(-1)]
                /// Y(-1)= k1*[(I-B*k1)(-1)]
                /// se k1=2*I,
                /// Y(-1)=I*(I-B*2*I)(-1)
                /// Y(-1)=2*((I-2*B)(-1))
                Matriz k = Matriz.Inversa(Matriz.Add(B, m32xM23));
                Matriz Y_inv = Matriz.Mul(2, Matriz.Inversa(Matriz.Sub(transformacao.I3(), Matriz.Mul(2, B))));

                k.PrintMatrix("matriz k: ");
                Y_inv.PrintMatrix("matriz Y inversa: ");

                System.Console.ReadLine();
                System.Environment.Exit(1);
            }

            public void Teste_07(AssercaoSuiteClasse assercao)
            {
                MyTransformacaoIsometrica transformacao = new MyTransformacaoIsometrica();
                Matriz m32xM23 = transformacao.m33.Clone();

                Matriz B = new Matriz(3, 3);
                for (int lin = 0; lin < 3; lin++)
                    for (int col = 0; col < 3; col++)
                        B.data[lin, col] = -m32xM23.data[lin, col];

                B.data[0, 0] += 2.0;
                B.data[1, 1] += 2.0;
                B.data[2, 2] += 2.0;


                /// Y(-1)=[(I-k*B)(-1)]*k,
                /// com k=[(m33+B)(-1)]
                /// Se k=2*I,
                /// Y(-1)=(I-2*B)(-1)*2,
                /// Y(-1)=2*(I-2*B)(-1).
            
                Matriz k = Matriz.Inversa(Matriz.Add(m32xM23, B));
                Matriz Y_inv = Matriz.Mul(2,Matriz.Inversa(Matriz.Sub(transformacao.I3(), Matriz.Mul(2, B))));

                k.PrintMatrix("matriz k: ");
                Y_inv.PrintMatrix("matriz Y_inv: ");

                System.Console.ReadLine();
                System.Environment.Exit(1);

            }



            public void TesteMatrizCorretiva_m23(AssercaoSuiteClasse assercao)
            {
                MyTransformacaoIsometrica transformacao = new MyTransformacaoIsometrica();
                MyTransformacaoIsometrica.isDebug = false;
                MyVetor.isDebug = true;

                Matriz Y_inv23_33 = transformacao.Matriz_Corretiva_m23();
                Matriz Y_inv32_33 = transformacao.Matriz_Corretiva_m32();

                MyVetor vt3d_initial = new MyVetor(1, 2, 3);
                MyVetor vt_2d = MyVetor.Mul(vt3d_initial, Matriz.Mul(Y_inv32_33, transformacao.m32));
                MyVetor vt3d_final = MyVetor.Mul(vt_2d, Matriz.Mul(transformacao.m23, Y_inv23_33));

                vt3d_initial.PrintVector("vetor inicial: ");
                vt3d_final.PrintVector("vetor final: ");

                System.Console.WriteLine("erro: " + imprecisao(vt3d_initial, vt3d_final));

                System.Console.ReadLine();
                System.Environment.Exit(0);
            }

            public void TesteMatrizCorretiva_m32(AssercaoSuiteClasse assercao)
            {
                MyTransformacaoIsometrica myTransformacao = new MyTransformacaoIsometrica();
                MyTransformacaoIsometrica.isDebug = false;
                MyVetor.isDebug = true;

                Matriz Y_inv = myTransformacao.Matriz_Corretiva_m32();

                MyVetor vt_3d_INI = new MyVetor(1, 2, 3);
                MyVetor vetor_2d = MyVetor.Mul(vt_3d_INI, Matriz.Mul(Y_inv, myTransformacao.m32));
                MyVetor vt_3d_FINI = MyVetor.Mul(vetor_2d, myTransformacao.m23);


                vt_3d_INI.PrintVector("vetor 3d inicial:");
                vt_3d_FINI.PrintVector("vetor 3d final: ");


                System.Console.WriteLine();
                System.Console.WriteLine();
                System.Console.WriteLine();

                vetor_2d.PrintVector("vetor 2d com corretiva: ");


                System.Console.WriteLine("erro: " + imprecisao(vt_3d_INI, vt_3d_FINI));


                System.Console.ReadLine();

                System.Environment.Exit(1);
            }



            public void TesteDeterminanteMatrizCorretiva(AssercaoSuiteClasse assercao)
            {
                MyTransformacaoIsometrica transformacao = new MyTransformacaoIsometrica();
                Matriz mt_33 = Matriz.Mul(transformacao.m32, transformacao.m23);

                mt_33.data[2, 2] *= 0.1;


                Matriz mt_33_inv = mt_33.Inversa();

                mt_33.PrintMatrix("matriz m32*m23:");
                mt_33_inv.PrintMatrix("matriz m32*m23 inversa corretiva: ");

                System.Console.ReadLine();
                System.Environment.Exit(0);
            }


            private double imprecisao(MyVetor vt_esperado, MyVetor vt_encontrado)
            {
                double erro = 0.0;
                for (int x = 0; x < vt_esperado.lenght(); x++) 
                    if (Math.Abs(vt_esperado.data[x] - vt_encontrado.data[x]) > erro)
                        erro = Math.Abs(vt_esperado.data[x] - vt_encontrado.data[x]);
                return erro;
            }

            public void TestGetPonto3D(AssercaoSuiteClasse assercao)
            {
                MyTransformacaoIsometrica myTransformacao = new MyTransformacaoIsometrica();
                MyTransformacaoIsometrica.isDebug = false;
                MyVetor.isDebug = true;


                MyVetor v3_ini = new MyVetor(1, 2, 11);
                MyVetor v2d = myTransformacao.GetPonto2D(v3_ini);
                MyVetor v3_COM_CORRETIVA_fini = myTransformacao.GetPonto3D(v2d);
                MyVetor v3_SEM_CORRETIVA_fini = MyVetor.Mul(v2d, myTransformacao.m23);
                v3_ini.PrintVector("vetor 3d inicial: ");
                v3_COM_CORRETIVA_fini.PrintVector("vetor 3d final COM corretiva: ");
                v3_SEM_CORRETIVA_fini.PrintVector("vetor 3d final SEM corretiva: ");

                System.Console.ReadLine();
                System.Environment.Exit(0);
            }

            public void TesteMatrizNaoQuadraticaInversa(AssercaoSuiteClasse assercao)
            {
                Matriz mt_teste = new Matriz(3, 2);
                mt_teste.data[0, 0] = 1.0;
                mt_teste.data[1, 0] = 0.0;
                mt_teste.data[2, 0] = 0.5;

                mt_teste.data[0, 1] = 0.0;
                mt_teste.data[1, 1] = 1.0;
                mt_teste.data[2, 1] = 0.5;

                MyTransformacaoIsometrica myTransformacao = new MyTransformacaoIsometrica();
                Matriz mt_teste_inversa = mt_teste.InversaNaoQuadratica();

                mt_teste.PrintMatrix("matriz m32: ");
                mt_teste_inversa.PrintMatrix("matriz m23: ");

                System.Console.ReadLine();
                System.Environment.Exit(0);
            }

            public void TesteGetPonto3d(AssercaoSuiteClasse assercao)
            {
                MyVetor.isDebug = false;


                MyTransformacaoIsometrica.isDebug = false;
                MyTransformacaoIsometrica.precisaoDigitos = 8;
                MyTransformacaoIsometrica transformacao = new MyTransformacaoIsometrica();



                MyVetor vt_3d_ini = new MyVetor(1, 2, 35);
                MyVetor vt_2D_ini = transformacao.GetPonto2D(vt_3d_ini);
                MyVetor vt_3d_fini = transformacao.GetPonto3D(vt_2D_ini);

                vt_3d_ini.PrintVector("vetor inicial:");
                vt_3d_fini.PrintVector("vetor final: ");

                System.Console.ReadLine();
                System.Environment.Exit(1);
                
            }


            public void Teste_Matriz23_corretiva(AssercaoSuiteClasse assercao)
            {
                MyTransformacaoIsometrica isometrica = new MyTransformacaoIsometrica();
                MyTransformacaoIsometrica.isDebug = true;
                Matriz m23_corretiva = isometrica.Matriz_Corretiva_m23();
                Matriz m23_semCorretiva = isometrica.M23();
                Matriz m32 = isometrica.M32();

                Matriz.PrintMatrix("m23 corretiva: ", m23_corretiva);
                Matriz.PrintMatrix("m23 sem corretiva: ", m23_semCorretiva);

                Matriz.PrintMatrix("identidade: ", Matriz.Add(Matriz.Mul(m32, m23_corretiva), isometrica.B));
                System.Console.ReadLine();

            }




        }

    }

}
