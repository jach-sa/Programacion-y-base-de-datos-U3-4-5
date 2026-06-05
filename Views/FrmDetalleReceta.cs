using System.Drawing;
using System.IO;
using System.Windows.Forms;
using ChefRecetasCS.Models;

namespace ChefRecetasCS.Views
{
    public class FrmDetalleReceta : Form
    {
        public FrmDetalleReceta(Receta r)
        {
            Text = r.Nombre;

            Size = new Size(700, 600);

            StartPosition =
                FormStartPosition.CenterScreen;

            BackColor =
                Color.Beige;

            var titulo = new Label
            {
                Text = r.Nombre,
                Font = new Font(
                    "Segoe UI",
                    22,
                    FontStyle.Bold),

                AutoSize = true,

                Location =
                    new Point(20, 20)
            };

            var pic = new PictureBox
            {
                Size = new Size(250, 180),

                Location =
                    new Point(20, 70),

                SizeMode =
                    PictureBoxSizeMode.Zoom,

                BorderStyle =
                    BorderStyle.FixedSingle
            };

            if (r.Foto != null)
            {
                pic.Image =
                    Image.FromStream(
                        new MemoryStream(r.Foto));
            }

            var txtInfo = new RichTextBox
            {
                ReadOnly = true,

                Width = 620,

                Height = 220,

                Location =
                    new Point(20, 280),

                Font = new Font(
                    "Segoe UI",
                    11),

                BackColor =
                    Color.White
            };

            txtInfo.Text =
                $"Descripción:\n{r.Descripcion}\n\n" +

                $"Tiempo: {r.TiempoPrepMin} min\n" +

                $"Porciones: {r.Porciones}\n" +

                $"Dificultad: {r.Dificultad}";

            Controls.Add(titulo);

            Controls.Add(pic);

            Controls.Add(txtInfo);
        }
    }
}