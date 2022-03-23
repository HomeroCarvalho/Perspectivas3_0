using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using Isometria;
using System.Drawing;
using System.Drawing.Drawing2D;
using MatrizesNaoQuadraticas;
using classeObjeto3dTestes;
using ProjetoMatrizesNaoQuadraticas.bibliotecasMatematicas;

namespace ProjetoPerspectivas3_0
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
            MyTransformacaoIsometrica transformacao = new MyTransformacaoIsometrica();

            MyVetor v3_00 = new MyVetor(0, dims.Y, 0);
            MyVetor v3_01 = new MyVetor(0, dims.Y, dims.Z);
            MyVetor v3_11 = new MyVetor(dims.X, dims.Y, dims.Z);
            MyVetor v3_10 = new MyVetor(dims.X, dims.Y, 0);

            
            
            Point pnt00 = CalcPointFromVector3D(v3_00);
            Point pnt01 = CalcPointFromVector3D(v3_01);
            Point pnt11 = CalcPointFromVector3D(v3_11);
            Point pnt10 = CalcPointFromVector3D(v3_10);

         

            Point max = CalcPointFromVector3D(dims);
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

        private Point CalcPointFromVector3D(MyVetor vt_3d)
        {
            MyVetor vt_2d = transformacao.GetPonto2D(vt_3d);
            Point pnt = new Point((int)vt_2d.X, (int)vt_2d.Y);

            return pnt;

        }
    }
}
