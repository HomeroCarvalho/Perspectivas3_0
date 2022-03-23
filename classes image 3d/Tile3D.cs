using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using System.Drawing.Drawing2D;
using ProjetoMatrizesNaoQuadraticas.bibliotecasMatematicas;
namespace classeObjeto3dTestes
{
    public class Tile3D
    {
        private static MyTransformacaoIsometrica transformacao = new MyTransformacaoIsometrica();

        private Objeto3d objeto3dTile;

        public Objeto3d GetObjeto3D()
        {
            return this.objeto3dTile;
        }
        public Tile3D(MyVetor dims)
        {
            MyVetor v3_00 = new MyVetor(0, dims.Y, 0);
            MyVetor v3_01 = new MyVetor(0, dims.Y, dims.Z);
            MyVetor v3_11 = new MyVetor(dims.X, dims.Y, dims.Z);
            MyVetor v3_10 = new MyVetor(dims.X, dims.Y, 0);

            
            
            Point pnt00 = CalcPointFromMyVetor(v3_00);
            Point pnt01 = CalcPointFromMyVetor(v3_01);
            Point pnt11 = CalcPointFromMyVetor(v3_11);
            Point pnt10 = CalcPointFromMyVetor(v3_10);

         

            Point max = CalcPointFromMyVetor(dims);
            Bitmap imgTile = new Bitmap((int)max.X, (int)max.Y);



            Graphics g = Graphics.FromImage(imgTile);
            Pen caneta = new Pen(Color.DarkGreen, 5);



            g.DrawLine(caneta, pnt00, pnt01);
            g.DrawLine(caneta, pnt01, pnt11);
            g.DrawLine(caneta, pnt11, pnt10);
            g.DrawLine(caneta, pnt10, pnt00);

            // cria o objeto 3d do tile, com a imagem temporaria com desenhos de linhas.
            objeto3dTile = new Objeto3d(imgTile, dims);

        }

        private Point CalcPointFromMyVetor(MyVetor vt_3d)
        {
            MyVetor vt_2d = MyVetor.Mul(vt_3d, transformacao.m32);
            Point pnt = new Point((int)vt_2d.X, (int)vt_2d.Y);

            return pnt;

        }
    }
}
