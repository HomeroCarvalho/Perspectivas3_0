using System;
using System.Collections.Generic;
using System.Runtime.CompilerServices;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using MathNet.Spatial.Euclidean;
using MatrizesNaoQuadraticas;

namespace Classes3D
{

    /// <summary>
    /// classe para especificação de uma base ortonormal, para fins de mudança de base.
    /// </summary>
    public class BaseOrtonormal
    {

        public MatrizesNaoQuadraticas.Vector3D eixoX { get; set; }
        public MatrizesNaoQuadraticas.Vector3D eixoY { get; set; }
        public MatrizesNaoQuadraticas.Vector3D eixoZ { get; set; }


        private Matrizes M_Mudanca_Base { get; set; }
        
        
        public MatrizesNaoQuadraticas.Vector3D GetEixoX()
        {
            return this.eixoX;
        }
        public MatrizesNaoQuadraticas.Vector3D GetEixoY()
        {
            return this.eixoY;
        }

        public MatrizesNaoQuadraticas.Vector3D GetEixoZ()
        {
            return this.eixoZ;
        }

        public BaseOrtonormal()
        {
            this.eixoX = new MatrizesNaoQuadraticas.Vector3D(1.0, 0.0, 0.0);
            this.eixoY = new MatrizesNaoQuadraticas.Vector3D(0.0, 1.0, 0.0);
            this.eixoZ = new MatrizesNaoQuadraticas.Vector3D(0.0, 0.0, 1.0);
        }

    

        /// <summary>
        /// calcula a matriz para mudança de base.
        /// </summary>
        /// <returns></returns>
        public Matrizes MatrizPadrao()
        {

            Matrizes mt_basePadrao = new Matrizes(3, 3);
            mt_basePadrao.GetMatriz()[0, 0] = this.GetEixoX().X;
            mt_basePadrao.GetMatriz()[0, 1] = this.GetEixoX().Y;
            mt_basePadrao.GetMatriz()[0, 2] = this.GetEixoX().Z;

            mt_basePadrao.GetMatriz()[1, 0] = this.GetEixoY().X;
            mt_basePadrao.GetMatriz()[1, 1] = this.GetEixoY().Y;
            mt_basePadrao.GetMatriz()[1, 2] = this.GetEixoY().Z;

            mt_basePadrao.GetMatriz()[2, 0] = this.GetEixoZ().X;
            mt_basePadrao.GetMatriz()[2, 1] = this.GetEixoZ().Y;
            mt_basePadrao.GetMatriz()[2, 2] = this.GetEixoZ().Z;

            return mt_basePadrao;

        }

        /// calcula a matriz de mudanca de base para a base ortonormal padrao.
        public Matrizes MatrizPadraoInversa()
        {
            /// ---> matriz mt com linhas formando os eixos.
            ///      mudança de base: v3_final=v3_ini*mt_padrao_inversa.
            ///      


            Matrizes mt_basePadrao = new Matrizes(3, 3);
            mt_basePadrao.GetMatriz()[0, 0] = this.GetEixoX().X;
            mt_basePadrao.GetMatriz()[0, 1] = this.GetEixoX().Y;
            mt_basePadrao.GetMatriz()[0, 2] = this.GetEixoX().Z;

            mt_basePadrao.GetMatriz()[1, 0] = this.GetEixoY().X;
            mt_basePadrao.GetMatriz()[1, 1] = this.GetEixoY().Y;
            mt_basePadrao.GetMatriz()[1, 2] = this.GetEixoY().Z;

            mt_basePadrao.GetMatriz()[2, 0] = this.GetEixoZ().X;
            mt_basePadrao.GetMatriz()[2, 1] = this.GetEixoZ().Y;
            mt_basePadrao.GetMatriz()[2, 2] = this.GetEixoZ().Z;




            Matrix<double> mt_padraoINVERSA = mt_basePadrao.GetMatriz().Inverse();
            
            Matrizes mt_mudanca_inversa = new Matrizes(3, 3);
            mt_mudanca_inversa.SetElement(0, 0, mt_padraoINVERSA[0, 0]);
            mt_mudanca_inversa.SetElement(0, 1, mt_padraoINVERSA[0, 1]);
            mt_mudanca_inversa.SetElement(0, 2, mt_padraoINVERSA[0, 2]);

            mt_mudanca_inversa.SetElement(1, 0, mt_padraoINVERSA[1, 0]);
            mt_mudanca_inversa.SetElement(1, 1, mt_padraoINVERSA[1, 1]);
            mt_mudanca_inversa.SetElement(1, 2, mt_padraoINVERSA[1, 2]);

            mt_mudanca_inversa.SetElement(2, 0, mt_padraoINVERSA[2, 0]);
            mt_mudanca_inversa.SetElement(2, 1, mt_padraoINVERSA[2, 1]);
            mt_mudanca_inversa.SetElement(2, 2, mt_padraoINVERSA[2, 2]);

            return mt_mudanca_inversa;

        }
        public MatrizesNaoQuadraticas.Vector3D MudancaDeBaseOrtonormalPadrao(MatrizesNaoQuadraticas.Vector3D ponto3D)
        {

            Vector<double> vectorDouble = Vector<double>.Build.Dense(new double[] { ponto3D.X, ponto3D.Y, ponto3D.Z });
            Vector<double> mt_vetor3 = vectorDouble * MatrizPadraoInversa().GetMatriz();


            return new MatrizesNaoQuadraticas.Vector3D(mt_vetor3[0], mt_vetor3[1], mt_vetor3[2]);
            
        }

        public MatrizesNaoQuadraticas.Vector3D MudancaDeBase(MatrizesNaoQuadraticas.Vector3D ponto3D)
        {

            Vector<double> vectorDouble = Vector<double>.Build.Dense(new double[] { ponto3D.X, ponto3D.Y, ponto3D.Z });
            Vector<double> vt3_entrada = vectorDouble * MatrizPadrao().GetMatriz();


            return new MatrizesNaoQuadraticas.Vector3D(vt3_entrada[0], vt3_entrada[1], vt3_entrada[2]);

        }

    }
}
