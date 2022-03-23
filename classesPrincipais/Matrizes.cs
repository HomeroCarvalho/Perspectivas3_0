using System;
using System.Collections.Generic;
using System.Text;
using MathNet.Numerics.LinearAlgebra;
using System.Drawing;

namespace MatrizesNaoQuadraticas
{

    public class Vector3D
    {
        public double X
        {
            get
            {
                return vector.X;
            }
            set
            {
                this.vector = new MathNet.Spatial.Euclidean.Vector3D(value, Y, Z);
            }
        }
        public double Y
        {
            get
            {
                return this.vector.Y;
            }
            set
            {
                this.vector = new MathNet.Spatial.Euclidean.Vector3D(X, value, Z);
            }
        }
        public double Z
        {
            get
            {
                return this.vector.Z;
            }
            set
            {
                this.vector = new MathNet.Spatial.Euclidean.Vector3D(X, Y, value);
            }
        }

        public Color cor { get; set; }

        public MathNet.Spatial.Euclidean.Vector3D vector;
    
        public Vector3D(double x, double y, double z)
        {
            this.vector = new MathNet.Spatial.Euclidean.Vector3D(x, y, z);
        }

        public Vector3D(double x, double y, double z, Color corVector)
        {
            this.vector = new MathNet.Spatial.Euclidean.Vector3D(x, y, z);
            this.cor = Color.FromArgb(cor.A, cor.R, cor.G, cor.B);
        }

        public static Vector3D Rotate(Vector3D vectorToRotate, double anguloXZ, double anguloXY, double anguloYZ)
        {
            MathNet.Spatial.Euclidean.Vector3D vector3d = new MathNet.Spatial.Euclidean.Vector3D(vectorToRotate.X, vectorToRotate.Y, vectorToRotate.Z);
            vector3d = vector3d.Rotate(new MathNet.Spatial.Euclidean.Vector3D(0, 1, 0), MathNet.Spatial.Units.Angle.FromDegrees(anguloXZ));
            vector3d = vector3d.Rotate(new MathNet.Spatial.Euclidean.Vector3D(1, 0, 0), MathNet.Spatial.Units.Angle.FromDegrees(anguloYZ));
            vector3d = vector3d.Rotate(new MathNet.Spatial.Euclidean.Vector3D(0, 0, 1), MathNet.Spatial.Units.Angle.FromDegrees(anguloXY));

            return new Vector3D(vector3d.X, vector3d.Y, vector3d.Z);
        }

        public Vector3D Clone()
        {
            return new Vector3D(X, Y, Z);
        }
        
     
        public static Vector3D operator -(Vector3D v1, Vector3D v2)
        {
            Vector3D vectorSaida = new Vector3D(v1.X - v2.X, v1.Y - v2.Y, v1.Z - v2.Z);


            vectorSaida.cor = Color.FromArgb(v1.cor.A, v1.cor.R, v1.cor.G, v1.cor.B);

            return vectorSaida;
        }

        public static Vector3D operator +(Vector3D v1, Vector3D v2)
        {
            Vector3D vectorSaida = new Vector3D(v1.X + v2.X, v1.Y + v1.Y, v1.Z + v2.Z);


            vectorSaida.cor = Color.FromArgb(v1.cor.A, v1.cor.R, v1.cor.G, v1.cor.B);

            return vectorSaida;
        }

        public override string ToString()
        {
            return "X= " + this.X.ToString("N1") + "  Y= " + this.Y.ToString("N1") + " Z=" + this.Z.ToString("N1");
        }
    }

    public class Vector2D
    {
        public double X
        {
            get
            {
                return vector.X;
            }
            set
            {
                vector = new MathNet.Spatial.Euclidean.Vector2D(value, Y);
            }
        }
        public double Y
        {
            get
            {
                return vector.Y;
            }
            set
            {
                this.vector = new  MathNet.Spatial.Euclidean.Vector2D(X, value);
            }
        }

        public Color cor { get; set; }


        public MathNet.Spatial.Euclidean.Vector2D vector;
        public Vector2D(double x, double y, Color corPonto)
        {
            this.vector = new  MathNet.Spatial.Euclidean.Vector2D(x, y);
            this.cor = Color.FromArgb(corPonto.A, corPonto.R, corPonto.G, corPonto.B);
        }
    }

    public class Matrizes
    {
        
        public Matrix<double> matrix;
        public static Matrizes matrixAux1 = null;
        static Matrizes matrixAux2 = null;
        public int linhas;
        public int colunas;
        public Matrizes(int lin, int col)
        {
            this.matrix = Matrix<double>.Build.Dense(lin, col);
            this.linhas = lin;
            this.colunas = col;
        }

        public Matrix<double>  GetMatriz()
        {
            return this.matrix;
        }

        public void SetElement(int lin, int col, double valor)
        {
            if ((lin < this.linhas) && (col < this.colunas))
                this.matrix[lin, col] = valor;
            else
                throw new Exception("numero de linhas ou numero de colunas invalido para o valor a ser atribuido ");
        }

        public void Fill(int fatorIndice)
        {
            for (int linha = 0; linha < this.matrix.RowCount; linha++) 
                for (int coluna = 0; coluna < this.matrix.ColumnCount; coluna++) 
                {
                    this.matrix[linha, coluna] = fatorIndice;
                    fatorIndice += 4 + 2;
                }
        }
    
        
        /// <summary>
        /// Calcula a Matriz Inversa nao quadrática, se a matriz que chamou este metodo
        /// tenha numero de linhas <> numero de colunas.
        /// Se tiver numero de linhas==numero de colunas, calcula a inversa quadrática.
        /// </summary>
        public Matrizes Inverse()
        {
            if (this.linhas > this.colunas)
            {
                /// exemplo do calculo da matriz nao quadratica, com utilizacao de matriz auxliar.
                /// M[3,2]*M[2,3]=I(3), M[3,2] é conhecida.
                /// Aux1[2,3]*M[3,2]*M[2,3]=Aux1[2,3]*I(3),
                /// Aux1M[2,2]*M[2,3]=Aux1[2,3]
                /// Aux1M32(-1)*Aux1M[2,2]*M[2,3]=Aux1M32(-1)*Aux1[2,3]
                /// M[2,3]=Aux1M32(-1)*Aux1[2,3]
                matrixAux1 = new Matrizes(this.colunas, this.linhas);
                matrixAux1.Fill(4);


                Matrix<double>  MAuxM32Inv = (matrixAux1.matrix * this.matrix).Inverse();
                Matrix<double>  MInv = MAuxM32Inv * matrixAux1.matrix;

                Matrizes MInversa = new Matrizes(this.colunas, this.linhas);
                MInversa.matrix = MInv;

                return MInversa;
            }
            else
            if (this.linhas < this.colunas)
            {
                /// exemplo do calculo da matriz nao quadratica, com utilizacao de matriz auxliar.
                /// M[3,2]*M[2,3]=I(3); M[2,3] é conhecida.
                /// M[3,2]*M[2,3]*Aux1[3,2]=I(3)*Aux1[3,2]
                /// M[3,2]*MAux1[2,2]=Aux1[3,2]
                /// M[3,2]*MAux1[2,2]*MAux1[2,2](-1)=Aux1[3,2]*MAux1[2,2](-1)
                /// M[3,2]=Aux1[3,2]*MAux1[2,2](-1)

                matrixAux1 = new Matrizes(this.colunas, this.linhas);
                matrixAux1.Fill(4);
                Matrix<double> MAuxM1Inv = (this.matrix * matrixAux1.matrix).Inverse();
                Matrix<double> MInv = matrixAux1.matrix * MAuxM1Inv;

                Matrizes MInversa = new Matrizes(this.colunas, this.linhas);
                MInversa.matrix = MInv;

                return MInversa;
            }
            else
            if (this.linhas == this.colunas)
            {
                Matrix<double> mInv = this.matrix.Inverse();

                Matrizes MInversa = new Matrizes(this.linhas, this.colunas);
                MInversa.matrix = mInv;
                return MInversa;
            }
            else
                return null;
        }

        /// <summary>
        /// calcula o quanto m1 está próxima de m2, em porcentagem: 0..1.
        /// matrizes m1 e m2 devem ter o mesmo numero de linhas e colunas.
        /// numero de linhas pode ser diferente do numero de colunas.
        /// </summary>
        public static double GrauSemelhanca(Matrizes m1, Matrizes m2, int rangeRandom)
        {
            if ((m1.linhas != m2.linhas) || (m1.colunas != m2.colunas))
                return -1.0;
            matrixAux1 = new Matrizes(m1.linhas, m1.colunas);
            matrixAux2 = new Matrizes(m1.linhas, m1.colunas);

            matrixAux1.Fill(rangeRandom);
            matrixAux2.Fill(rangeRandom);
            double d1 = (matrixAux1.matrix * m1.matrix * matrixAux2.matrix)[0, 0];
            double d2 = (matrixAux1.matrix * m2.matrix * matrixAux2.matrix)[0, 0];

            return d1 / d2;

        }

        /// <summary>
        /// calcula uma matriz bijetora, tal que t(t(-1)(x))=x, t é a função matriz bijetora,
        /// </summary>
        /// <param name="M32_inicial">matriz inicial, de quaisquer dimensões</param>
        /// <returns></returns>
        public Matrizes TreinamentoMatrizBiJetora()
        {
            Matrizes M32_inicial = this;

            for (int x = 0; x < 5; x++) 
            {
                Matrizes M23_intermediaria = M32_inicial.Inverse();
                Matrizes M32_final = M23_intermediaria.Inverse();
        
                Matrizes M_erro = M32_final - M32_inicial;


                M32_inicial = M32_final - M_erro;
            }

            return M32_inicial;
        }

        public static Matrizes operator -(Matrizes m1, Matrizes m2)
        {
            if ((m1.linhas != m2.linhas) || (m1.colunas != m2.colunas))
                throw new Exception("matrizes de dimensoes diferentes, em operacao menos.");
            else
            {
                Matrizes mResult = new Matrizes(m1.linhas, m1.colunas);
                mResult.matrix = m1.matrix - m2.matrix;

                return mResult;

            }
        }

        public static Matrizes operator +(Matrizes m1, Matrizes m2)
        {
            if ((m1.linhas != m2.linhas) || (m1.colunas != m2.colunas))
                throw new Exception("matrizes de dimensoes diferentes, em operacao menos.");
            else
            {
                Matrizes mResult = new Matrizes(m1.linhas, m1.colunas);
                mResult.matrix = m1.matrix + m2.matrix;

                return mResult;

            }
        }

        public static Matrizes operator *(Matrizes m1, Matrizes m2)
        {
            if (m1.colunas != m2.linhas)
                throw new Exception("matrizes de dimensoes nao compativel com operacao de multiplicacao de matrizes");
            else
            {
                Matrizes mResult = new Matrizes(m1.linhas, m2.colunas);
                mResult.matrix = m1.matrix * m2.matrix;
                return mResult;
            }
        }
    }
}
