using System.Collections.Generic;
using System.Drawing;

namespace META_GAME.Utils
{
    /// <summary>
    /// classe para recortar imagens, se a cor do Ponto minimo ou Ponto maximo fazer um match
    /// especificado no [MatchColor]. Os pontos mínimo e máximo entrarão no recorte da imagem.
    /// </summary>
    public class BitmapCutting
    {
        /// <summary>
        /// imagem a ser recortado.
        /// </summary>
        private Bitmap imgToCut;


        /// <summary>
        /// Seta a imagem para recortar.
        /// </summary>
        /// <param name="img"></param>
        public void SetImage(Bitmap img)
        {
            this.imgToCut = new Bitmap(img);
        }
        /// <summary>
        /// construtor, aceita a imagem a ser recortada.
        /// </summary>
        /// <param name="cena"></param>
        public BitmapCutting(Bitmap cena)
        {
            this.imgToCut = cena;
        } // BitmapCut()

        /// <summary>
        /// recorta a imagem, se os pontos mínimo e máximo fizer um Match
        /// especificado no método [MatchColor()]
        /// </summary>
        /// <returns></returns>
        public Bitmap Cut()
        {
            List<Point> cutXBegin = new List<Point>();
            List<Point> cutXEnd = new List<Point>();
            List<Point> cutYBegin = new List<Point>();
            List<Point> cutYEnd = new List<Point>();
            for (int y = 0; y < imgToCut.Height; y++)
            {
                for (int x = 0; x < imgToCut.Width; x++)
                {
                    if (!MatchColor(imgToCut.GetPixel(x, y)))
                    {
                        cutXBegin.Add(new Point(x, y));
                        break;
                    } // if
                } // for x
                for (int x = imgToCut.Width - 1; x >= 0; x--)
                {
                    if (!MatchColor(imgToCut.GetPixel(x, y)))
                    {
                        cutXEnd.Add(new Point(x, y));
                        break;
                    } // if
                }
            } // for y
            for (int x = 0; x < imgToCut.Width; x++)
            {
                for (int y = 0; y < imgToCut.Height; y++)
                {
                    if (!MatchColor(imgToCut.GetPixel(x, y)))
                    {
                        cutYBegin.Add(new Point(x, y));
                        break;
                    } // if
                } // for y
                for (int y = imgToCut.Height - 1; y >= 0; y--)
                {
                    if (!MatchColor(imgToCut.GetPixel(x, y)))
                    {
                        cutYEnd.Add(new Point(x, y));
                        break;
                    } // if
                } // for y
            } // for x
            int minX = 100000;
            int maxX = -1;
            int minY = 100000;
            int maxY = -1;
            for (int i = 0; i < cutXBegin.Count; i++)
            {
                if (cutXBegin[i].X < minX)
                    minX = cutXBegin[i].X;
            } // for i
            for (int i = 0; i < cutXEnd.Count; i++)
            {
                if (cutXEnd[i].X > maxX)
                    maxX = cutXEnd[i].X;
            } // for i
            
            
            
            for (int i = 0; i < cutYBegin.Count; i++)
            {
                if (cutYBegin[i].Y < minY)
                    minY = cutYBegin[i].Y;
            } // for i
            
            
            
            for (int i = 0; i < cutYEnd.Count; i++)
            {
                if (cutYEnd[i].Y > maxY)
                    maxY = cutYEnd[i].Y;
            } // for i
            
            Bitmap imgToCutFinal = new Bitmap(maxX - minX + 2, maxY - minY + 2);
            Graphics g = Graphics.FromImage(imgToCutFinal);
            g.DrawImage(
                this.imgToCut,
                new Rectangle(0, 0, maxX - minX, maxY - minY),
                new Rectangle(minX, minY, maxX - minX, maxY - minY),
                GraphicsUnit.Pixel);
            return imgToCutFinal;
        } // Cut()

        /// <summary>
        /// retorna [true] se a cor de entrada tiver dentro das especificações contido
        /// no corpo deste método.
        /// </summary>
        /// <param name="cor">cor a ser verificada.</param>
        /// <returns></returns>
        private bool MatchColor(Color cor)
        {
            return (cor.A < 128);
        } // MatchColor()
    } // class BitmapCut
} // namespace META_GAME.Utils
