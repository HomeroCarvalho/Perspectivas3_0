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


        public Matriz m32
        { 
            get;
            set;
        }
        
        


        public Matriz m23 
        { 
            get;
            set;
        }


        public static int precisaoDigitos = 21;

        public static bool isDebug = false;

        

        double VARIAVEL_LIVRE = 2.0;


  
        
        public MyTransformacaoIsometrica()
        {
            this.m32 = Mt_32_modificada();
            this.m23 = Mt_23_modificada();


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


        public MyVetor GetPonto2D(MyVetor vetor3d)
        {

            Matriz mt_vetor3d = MyVetor.ToMatriz(vetor3d);

            Matriz mt_result = Matriz.Mul(mt_vetor3d, Mt_32_modificada());




            return mt_result.ToVetor();
        }


        public MyVetor GetPonto3D(MyVetor vetor2d)
        {

            // um terceira coordenada, possibilitando a multiplicação pela matriz [3,3]: m23!
            MyVetor vetor2d_initial = vetor2d.Clone();

            // acrescenta a variavel livre,  se não estiver na transformação currente.
            if (vetor2d_initial.data.Count == 2)
                vetor2d_initial.data.Add(VARIAVEL_LIVRE);
            




            Matriz mt_vetor2d_final = MyVetor.ToMatriz(vetor2d_initial);




            Matriz mt_result = Matriz.Mul(mt_vetor2d_final, Mt_23_modificada());
            return mt_result.ToVetor();

        }

 


   
        public Matriz Mt_23_modificada()
        {
            Matriz mt23_nao_modificada = this.M32().InversaNaoQuadratica();
            Matriz mt23_modificada = new Matriz(3, 3);
            for (int row = 0; row < 2; row++)
                for (int col = 0; col < 3; col++)
                    mt23_modificada.data[row, col] = mt23_nao_modificada.data[row, col];

            mt23_modificada.data[2, 0] = 0.0;
            mt23_modificada.data[2, 1] = 0.0;
            mt23_modificada.data[2, 2] = VARIAVEL_LIVRE;

            return mt23_modificada;

        }

        public Matriz Mt_32_modificada()
        {
            Matriz mt_23_modificada = Mt_23_modificada().Inversa();
            return mt_23_modificada;
        }


  






    

        public class Testes : SuiteClasseTestes
        {

            public Testes() : base("teste para transformacao com matrizes nao quadraticas.")
            {

            }




            public void TesteGetPonto2d(AssercaoSuiteClasse assercao)
            {
                MyTransformacaoIsometrica isometrica = new MyTransformacaoIsometrica();






                MyVetor vetor3d_initial = new MyVetor(3, 5, 8);

                MyVetor vetor2d_intermediario = isometrica.GetPonto2D(vetor3d_initial);


                MyVetor vetor3d_final = isometrica.GetPonto3D(vetor2d_intermediario);





                PrintVector("vetor 3d initial: ", vetor3d_initial, 3);
                PrintVector("vetor 3d final: ", vetor3d_final, 3);
                PrintVector("vetor 2d intermediario: ", vetor2d_intermediario, 2);





                System.Console.WriteLine("imprecisao: " + imprecisao(vetor3d_initial, vetor3d_final));



                System.Console.ReadLine();



            }

            public void TesteGetPonto3d(AssercaoSuiteClasse assercao)
            {
                MyVetor vetor2d_initial = new MyVetor(3, 5);

                MyTransformacaoIsometrica isometrica = new MyTransformacaoIsometrica();

                MyVetor vetor3d_initial = isometrica.GetPonto3D(vetor2d_initial);

                MyVetor vetor2d_final = isometrica.GetPonto2D(vetor3d_initial);



                PrintVector("vetor 2d initial:", vetor2d_initial, 2);
                PrintVector("vetor 2d final:", vetor2d_final, 2);
                PrintVector("vetor 3d intermediario:", vetor3d_initial, 3);




                System.Console.WriteLine("imprecisao: " + imprecisao(vetor2d_initial, vetor2d_final));


                System.Console.ReadLine();

            }



            private double imprecisao(MyVetor vt_esperado, MyVetor vt_encontrado)
            {
                double erro = 0.0;
                for (int x = 0; x < vt_esperado.lenght(); x++) 
                    if (Math.Abs(vt_esperado.data[x] - vt_encontrado.data[x]) > erro)
                        erro = Math.Abs(vt_esperado.data[x] - vt_encontrado.data[x]);
                return erro;
            }


            public void PrintVector(string caption, MyVetor v, int numberCoords)
            {
                System.Console.WriteLine(caption);
                System.Console.Write("<");
                for (int x = 0; x < numberCoords - 1; x++)
                    System.Console.Write(v.data[x].ToString("N5") + ",");

                System.Console.Write(v.data[numberCoords - 1].ToString("N5"));

                System.Console.WriteLine(">");


            }



        }

    }

}
