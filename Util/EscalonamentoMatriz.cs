using System;
using MathNet.Numerics.LinearAlgebra;
using Testes;

namespace ProjetoMatrizesNaoQuadraticas.bibliotecasMatematicas
{
    public class EscalonamentoMatriz
    {

        public Matrix<double> mt_scaled { get; private set; }


        public EscalonamentoMatriz(Matrix<double> mt_toEscale)
        {
            if (mt_toEscale.RowCount != mt_toEscale.ColumnCount)
                throw new Exception("Matriz nao quadratica, impossivel de escalonar.");
            this.mt_scaled = mt_toEscale;
        }

        public void ScaleRows()
        {
            for (int pivot = 0; pivot < mt_scaled.RowCount; pivot++)
                for (int lineToScale = 0; lineToScale < mt_scaled.RowCount; lineToScale++)

                    this.EscalonaUmaLinha(pivot, lineToScale);

        }


        public void ScaleColumns()
        {

            for (int pivot = 0; pivot < mt_scaled.RowCount; pivot++)
                for (int colToScale = 0; colToScale < mt_scaled.ColumnCount; colToScale++)
                    this.EscalonaUmaColuna(pivot, colToScale);

        }



        private void EscalonaUmaLinha(int pivot, int indexLineToScale)
        {
            Vector<double> linhaAEscalonar = mt_scaled.Row(indexLineToScale);
            Vector<double> linhaPivo = mt_scaled.Row(pivot);

            if ((indexLineToScale >= (pivot + 1)) && (linhaAEscalonar[pivot] != 0.0)) // se o elemento da linha a escalonar for 0.0, não há o que fazer, já está escalonado neste elemento.
            {
                Vector<double> linhaEscalonada = linhaPivo + (-linhaPivo[pivot] / linhaAEscalonar[pivot]) * linhaAEscalonar;

                mt_scaled = mt_scaled.RemoveRow(indexLineToScale);
                mt_scaled = mt_scaled.InsertRow(indexLineToScale, linhaEscalonada);
            }

        }


        private void EscalonaUmaColuna(int pivot, int indexColToScale)
        {
            Vector<double> colunaAEscalonar = mt_scaled.Column(indexColToScale);
            Vector<double> colunaPivo = mt_scaled.Column(pivot);

            if ((indexColToScale >= (pivot + 1)) && (colunaAEscalonar[pivot] != 0.0))  // se o elemento da coluna a escalonar for 0.0, não há o que fazer, já está escalonado neste elemento.
            {
                Vector<double> colunaEscalonada = colunaPivo + (-colunaPivo[pivot] / colunaAEscalonar[pivot]) * colunaAEscalonar;

                mt_scaled = mt_scaled.RemoveColumn(indexColToScale);
                mt_scaled = mt_scaled.InsertColumn(indexColToScale, colunaEscalonada);
            }
        }


        public class Teste : SuiteClasseTestes
        {
            public Teste() : base("teste de escalonamento de matriz")
            {
            }

 
        }

    }
}
