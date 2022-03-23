using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;
using ProjetoMatrizesNaoQuadraticas.bibliotecasMatematicas;

namespace classeObjeto3dTestes
{

    /// <summary>
    /// classe para especificação de uma base ortonormal, para fins de mudança de base.
    /// </summary>
    public class BaseOrtonormal
    {

        public MyVetor eixoX { get; set; }
        public MyVetor eixoY { get; set; }
        public MyVetor eixoZ { get; set; }


        private Matrizes M_Mudanca_Base { get; set; }

        private static Matrizes mt_basePadrao { get; set; }

        public MyVetor GetEixoX()
        {
            return this.eixoX;
        }
        public MyVetor GetEixoY()
        {
            return this.eixoY;
        }

        public MyVetor GetEixoZ()
        {
            return this.eixoZ;
        }

        public BaseOrtonormal()
        {
            this.eixoX = new MyVetor(1.0, 0.0, 0.0);
            this.eixoY = new MyVetor(0.0, 1.0, 0.0);
            this.eixoZ = new MyVetor(0.0, 0.0, 1.0);
        }

    

        /// <summary>
        /// calcula a matriz para mudança de base.
        /// </summary>
        /// <returns></returns>
        public Matriz MatrizPadrao()
        {

            Matriz mt_basePadrao = new Matriz(3, 3);
            mt_basePadrao.data[0, 0] = eixoX.X;
            mt_basePadrao.data[0, 1] = eixoX.Y;
            mt_basePadrao.data[0, 2] = eixoX.Z;

            mt_basePadrao.data[1, 0] = eixoY.X;
            mt_basePadrao.data[1, 1] = eixoY.Y;
            mt_basePadrao.data[1, 2] = eixoY.Z;

            mt_basePadrao.data[2, 0] = eixoZ.X;
            mt_basePadrao.data[2, 1] = eixoZ.Y;
            mt_basePadrao.data[2, 2] = eixoZ.Z;

            return mt_basePadrao;

        }

        /// calcula a matriz de mudanca de base para a base ortonormal padrao.
        public Matriz MatrizPadraoInversa()
        {
            /// ---> matriz mt com linhas formando os eixos.
            ///      mudança de base: v3_final=v3_ini*mt_padrao_inversa.
            ///      


            Matriz  mt_basePadrao = new Matriz(3, 3);
            mt_basePadrao.data[0, 0] = eixoX.X;
            mt_basePadrao.data[0, 1] = eixoX.Y;
            mt_basePadrao.data[0, 2] = eixoX.Z;

            mt_basePadrao.data[1, 0] = eixoY.X;
            mt_basePadrao.data[1, 1] = eixoY.Y;
            mt_basePadrao.data[1, 2] = eixoY.Z;

            mt_basePadrao.data[2, 0] = eixoZ.X;
            mt_basePadrao.data[2, 1] = eixoZ.Y;
            mt_basePadrao.data[2, 2] = eixoZ.Z;




            return mt_basePadrao.Inversa();
           
        }
        public MyVetor ParaBaseOrtogonalPadrao(MyVetor ponto3D)
        {
            return MyVetor.Mul(ponto3D, MatrizPadraoInversa());
        }

        public MyVetor ParaBaseDosEixos(MyVetor ponto3D)
        {

            return MyVetor.Mul(ponto3D, MatrizPadrao());
        }

    }
}
