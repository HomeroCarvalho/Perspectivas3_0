using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Drawing;
using Isometria;
using MatrizesNaoQuadraticas;
namespace Classes3D.Util
{
    public class UtilTiles
    {
        private static TransformacaoIsometricaNaoQuadratica transformacao = new TransformacaoIsometricaNaoQuadratica();

        public static Size CalcSizeFromVetor3d(Vector3D location)
        {
     
            
            Vector2D vt_size = transformacao.GetPonto2D(location);
            Size size = new Size((int)vt_size.X, (int)vt_size.Y);
            return size;
        }
    } // class
} // namespace
