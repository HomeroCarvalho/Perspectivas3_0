using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using Testes;
using MathNet.Numerics.LinearAlgebra;

namespace ProjetoMatrizesNaoQuadraticas.bibliotecasMatematicas
{
    public partial class MyTransformacaoIsometrica
    {
        public class TiraDependenciaLinearDeMatriz
        {
            public TiraDependenciaLinearDeMatriz()
            {

            }


            /// <summary>
            /// cria uma matriz corretiva tal que (m32*m23+B)=I
            /// </summary>
            public Matriz MakeMatrizTiraDependencia()
            {


                MyTransformacaoIsometrica transformacao = new MyTransformacaoIsometrica();
                MyTransformacaoIsometrica.precisaoDigitos = 5;


                Matriz M32xM23 = Matriz.Mul(transformacao.M32(), transformacao.M23());
                Matriz B = new Matriz(3, 3);

                B.data[0, 0] = -M32xM23.data[0, 0] + 1.0;
                B.data[1, 1] = -M32xM23.data[1, 1] + 1.0;
                B.data[2, 2] = -M32xM23.data[2, 2] + 1.0;

                B.data[0, 1] = -M32xM23.data[0, 1];
                B.data[0, 2] = -M32xM23.data[0, 2];


                B.data[1, 0] = -M32xM23.data[1, 0];
                B.data[1, 2] = -M32xM23.data[1, 2];


                B.data[2, 0] = -M32xM23.data[2, 0];
                B.data[2, 1] = -M32xM23.data[2, 1];

                return B;


            }

            public class Testes : SuiteClasseTestes
            {
                public Testes() : base("Testes para matriz corretiva em transformacoes")
                {
                }


                public void TesteTransformacoes3d_2d_3d(AssercaoSuiteClasse assercao)
                {

                    /// formula de convergência: p3_fini=p3_ini*m32*m23+p3_ini*B,
                    /// ou seja: p3_fini=p3_ini*(m32*m23+B),
                    /// o problema é que não temos [p3_ini] durante os calculos intermediarios, 
                    /// envolvendo p2=p3_ini*m32, e p3_fini_aproximado=p2*m23,e delta=0.00001*p3_fini_aproximado*B,
                    /// e p3_fini= p3_fini_aproximado- delta; (sinal negativo porque o determinante de |m32*m23|<0.0)


                    MyTransformacaoIsometrica trans = new MyTransformacaoIsometrica();
                    Matriz M32xM23 = Matriz.Mul(trans.M32(), trans.M23());
                    Matriz.isDebug = true;
                    Matriz m32 = trans.m32;
                    Matriz m23 = trans.m23;

                    // calculo de uma matriz m23 corretiva.
                    Matriz m33 = Matriz.Mul(m32, m23);


                    //gera a matriz de correção.
                    TiraDependenciaLinearDeMatriz corretiva = new TiraDependenciaLinearDeMatriz();
                    Matriz B = corretiva.MakeMatrizTiraDependencia();



                    // gera o vetor de testes. preencher  o vetor.
                    MyVetor ponto3d_ini = new MyVetor(3);
                    ponto3d_ini.data[0] = 1.0;
                    ponto3d_ini.data[1] = 2.0;
                    ponto3d_ini.data[2] = 3.0;

                    Matriz mt_ponto3d_ini_base = MyVetor.ToMatriz(ponto3d_ini);

                    // o ponto de partida é um vetor 2d.
                    Matriz mt_pont2d_partida = Matriz.Mul(mt_ponto3d_ini_base, m32);

                    // vetor 3d iterativo, com aproximação baseada no vetor 2d de partida.
                    // é conhecido: ponto2d, B, m32,m23.
                    Matriz mt_ponto3d_ini_iterativo = Matriz.Mul(mt_pont2d_partida, m23);
                    Matriz mt_ponto3d_previo = new Matriz(1, 3);

                    Matriz.PrintMatrix("mt_ponto3d iterativo: ", mt_ponto3d_ini_iterativo);

                    Matriz mt_ponto3d_fini = new Matriz(1, 3);

                    for (int x = 0; x < 1000; x++)
                    {

                      
                        Matriz delta = Matriz.Mul(0.1, Matriz.Mul(mt_ponto3d_ini_iterativo, B));

                        mt_ponto3d_previo = mt_ponto3d_ini_iterativo.Clone();
                        mt_ponto3d_ini_iterativo = Matriz.Sub(mt_ponto3d_ini_iterativo, delta);

                        if (EqualsMatrix(mt_ponto3d_ini_iterativo, mt_ponto3d_previo))
                        {
                            mt_ponto3d_previo.PrintMatrix("ponto 3d iterativo previo: ");
                            mt_ponto3d_ini_iterativo.PrintMatrix("mt_ponto3d iterativo: ");
                            mt_ponto3d_ini_base.PrintMatrix("mt ponto3d objetivo: ");

                            System.Console.WriteLine("calculo convergiu.");
                            System.Console.WriteLine("numero de iterações: " + x.ToString());
                            System.Console.ReadLine();
                            System.Environment.Exit(1);
                        }

                     
                    }

                    System.Console.WriteLine("calculo nao convergiu.");
                    System.Console.ReadLine();
                    System.Environment.Exit(1);
                }


                private bool EqualsMatrix(Matriz m1, Matriz m2)
                {
                    if ((m1.lines != m2.lines) || (m1.columns != m2.columns))
                        return false;
                    else
                    {
                        for (int lin = 0; lin < m1.lines; lin++)
                            for (int col = 0; col < m2.columns; col++)
                                if (Math.Abs(Math.Abs(Math.Round(m1.data[lin, col])) - Math.Abs(Math.Round(m2.data[lin, col]))) > 0.1)
                                    return false;

                        return true;
                    }
                }
             
              

                public void TesteIdentidadeCorretiva(AssercaoSuiteClasse assercao)
                {
                    TiraDependenciaLinearDeMatriz geracao = new TiraDependenciaLinearDeMatriz();
                    Matriz B = geracao.MakeMatrizTiraDependencia();


                    MyTransformacaoIsometrica isometrica = new MyTransformacaoIsometrica();
                    Matriz M32xM23 = Matriz.Mul(isometrica.M32(), isometrica.M23());



                    Matriz isometricaDaOperacao = Matriz.Add(M32xM23, B);

                    Matriz.PrintMatrix("identidade (M32xM23+B)", isometricaDaOperacao);

                    System.Console.ReadLine();


                }

            }

        }


    }

}
