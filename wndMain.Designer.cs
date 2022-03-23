
namespace ProjetoPerspectivas3_0
{
    partial class wndMain
    {
        /// <summary>
        /// Variável de designer necessária.
        /// </summary>
        private System.ComponentModel.IContainer components = null;

        /// <summary>
        /// Limpar os recursos que estão sendo usados.
        /// </summary>
        /// <param name="disposing">true se for necessário descartar os recursos gerenciados; caso contrário, false.</param>
        protected override void Dispose(bool disposing)
        {
            if (disposing && (components != null))
            {
                components.Dispose();
            }
            base.Dispose(disposing);
        }

        #region Código gerado pelo Windows Form Designer

        /// <summary>
        /// Método necessário para suporte ao Designer - não modifique 
        /// o conteúdo deste método com o editor de código.
        /// </summary>
        private void InitializeComponent()
        {
            this.menuStrip1 = new System.Windows.Forms.MenuStrip();
            this.filesToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.loadImageToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.saveImageProcessedToolStripMenuItem = new System.Windows.Forms.ToolStripMenuItem();
            this.BaseOrtonormalView = new System.Windows.Forms.ListView();
            this.label2 = new System.Windows.Forms.Label();
            this.pctBxImageIn = new System.Windows.Forms.PictureBox();
            this.pctBxImageOut = new System.Windows.Forms.PictureBox();
            this.lblImageIn = new System.Windows.Forms.Label();
            this.lblImageOut = new System.Windows.Forms.Label();
            this.pctBxImagesRotated = new System.Windows.Forms.PictureBox();
            this.btnCreateRotateImages = new System.Windows.Forms.Button();
            this.cmbBxPlaneRotation = new System.Windows.Forms.ComboBox();
            this.lblRotationAngle = new System.Windows.Forms.Label();
            this.txtBxAngleRotation = new System.Windows.Forms.TextBox();
            this.lblAnimation = new System.Windows.Forms.Label();
            this.pctBxAnimationObjectRotate = new System.Windows.Forms.PictureBox();
            this.btnCreateAnimation = new System.Windows.Forms.Button();
            this.listView1 = new System.Windows.Forms.ListView();
            this.lbPlaneRotate = new System.Windows.Forms.Label();
            this.lstBxInstructions = new System.Windows.Forms.ListBox();
            this.menuStrip1.SuspendLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctBxImageIn)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBxImageOut)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBxImagesRotated)).BeginInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBxAnimationObjectRotate)).BeginInit();
            this.SuspendLayout();
            // 
            // menuStrip1
            // 
            this.menuStrip1.Items.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.filesToolStripMenuItem});
            this.menuStrip1.Location = new System.Drawing.Point(0, 0);
            this.menuStrip1.Name = "menuStrip1";
            this.menuStrip1.Size = new System.Drawing.Size(1264, 24);
            this.menuStrip1.TabIndex = 0;
            this.menuStrip1.Text = "menuStrip1";
            // 
            // filesToolStripMenuItem
            // 
            this.filesToolStripMenuItem.DropDownItems.AddRange(new System.Windows.Forms.ToolStripItem[] {
            this.loadImageToolStripMenuItem,
            this.saveImageProcessedToolStripMenuItem});
            this.filesToolStripMenuItem.Name = "filesToolStripMenuItem";
            this.filesToolStripMenuItem.Size = new System.Drawing.Size(42, 20);
            this.filesToolStripMenuItem.Text = "Files";
            // 
            // loadImageToolStripMenuItem
            // 
            this.loadImageToolStripMenuItem.Name = "loadImageToolStripMenuItem";
            this.loadImageToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.loadImageToolStripMenuItem.Text = "Load Image";
            this.loadImageToolStripMenuItem.Click += new System.EventHandler(this.loadImageToolStripMenuItem_Click);
            // 
            // saveImageProcessedToolStripMenuItem
            // 
            this.saveImageProcessedToolStripMenuItem.Name = "saveImageProcessedToolStripMenuItem";
            this.saveImageProcessedToolStripMenuItem.Size = new System.Drawing.Size(190, 22);
            this.saveImageProcessedToolStripMenuItem.Text = "Save Image Processed";
            this.saveImageProcessedToolStripMenuItem.Click += new System.EventHandler(this.saveImageProcessedToolStripMenuItem_Click);
            // 
            // BaseOrtonormalView
            // 
            this.BaseOrtonormalView.HideSelection = false;
            this.BaseOrtonormalView.Location = new System.Drawing.Point(146, 84);
            this.BaseOrtonormalView.Name = "BaseOrtonormalView";
            this.BaseOrtonormalView.Size = new System.Drawing.Size(121, 97);
            this.BaseOrtonormalView.TabIndex = 3;
            this.BaseOrtonormalView.UseCompatibleStateImageBehavior = false;
            // 
            // label2
            // 
            this.label2.AutoSize = true;
            this.label2.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.label2.Location = new System.Drawing.Point(12, 84);
            this.label2.Name = "label2";
            this.label2.Size = new System.Drawing.Size(128, 16);
            this.label2.TabIndex = 4;
            this.label2.Text = "Ortonormal Base:";
            // 
            // pctBxImageIn
            // 
            this.pctBxImageIn.Location = new System.Drawing.Point(343, 84);
            this.pctBxImageIn.Name = "pctBxImageIn";
            this.pctBxImageIn.Size = new System.Drawing.Size(158, 137);
            this.pctBxImageIn.TabIndex = 5;
            this.pctBxImageIn.TabStop = false;
            // 
            // pctBxImageOut
            // 
            this.pctBxImageOut.Location = new System.Drawing.Point(543, 84);
            this.pctBxImageOut.Name = "pctBxImageOut";
            this.pctBxImageOut.Size = new System.Drawing.Size(158, 137);
            this.pctBxImageOut.TabIndex = 6;
            this.pctBxImageOut.TabStop = false;
            // 
            // lblImageIn
            // 
            this.lblImageIn.AutoSize = true;
            this.lblImageIn.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImageIn.Location = new System.Drawing.Point(379, 247);
            this.lblImageIn.Name = "lblImageIn";
            this.lblImageIn.Size = new System.Drawing.Size(71, 18);
            this.lblImageIn.TabIndex = 7;
            this.lblImageIn.Text = "Image In";
            // 
            // lblImageOut
            // 
            this.lblImageOut.AutoSize = true;
            this.lblImageOut.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblImageOut.Location = new System.Drawing.Point(567, 247);
            this.lblImageOut.Name = "lblImageOut";
            this.lblImageOut.Size = new System.Drawing.Size(90, 18);
            this.lblImageOut.TabIndex = 8;
            this.lblImageOut.Text = "Image  Out";
            // 
            // pctBxImagesRotated
            // 
            this.pctBxImagesRotated.Location = new System.Drawing.Point(15, 286);
            this.pctBxImagesRotated.Name = "pctBxImagesRotated";
            this.pctBxImagesRotated.Size = new System.Drawing.Size(773, 103);
            this.pctBxImagesRotated.TabIndex = 9;
            this.pctBxImagesRotated.TabStop = false;
            // 
            // btnCreateRotateImages
            // 
            this.btnCreateRotateImages.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateRotateImages.Location = new System.Drawing.Point(15, 187);
            this.btnCreateRotateImages.Name = "btnCreateRotateImages";
            this.btnCreateRotateImages.Size = new System.Drawing.Size(252, 23);
            this.btnCreateRotateImages.TabIndex = 10;
            this.btnCreateRotateImages.Text = "Create Rotate Images";
            this.btnCreateRotateImages.UseVisualStyleBackColor = true;
            this.btnCreateRotateImages.Click += new System.EventHandler(this.btnCreateRotateImages_Click);
            // 
            // cmbBxPlaneRotation
            // 
            this.cmbBxPlaneRotation.FormattingEnabled = true;
            this.cmbBxPlaneRotation.Items.AddRange(new object[] {
            "rotate plane XZ",
            "rotate plane XY",
            "rotate planeYZ"});
            this.cmbBxPlaneRotation.Location = new System.Drawing.Point(15, 43);
            this.cmbBxPlaneRotation.Name = "cmbBxPlaneRotation";
            this.cmbBxPlaneRotation.Size = new System.Drawing.Size(298, 21);
            this.cmbBxPlaneRotation.TabIndex = 11;
            this.cmbBxPlaneRotation.SelectedIndexChanged += new System.EventHandler(this.comboBox1_SelectedIndexChanged);
            // 
            // lblRotationAngle
            // 
            this.lblRotationAngle.AutoSize = true;
            this.lblRotationAngle.Font = new System.Drawing.Font("Microsoft Sans Serif", 9F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblRotationAngle.Location = new System.Drawing.Point(340, 43);
            this.lblRotationAngle.Name = "lblRotationAngle";
            this.lblRotationAngle.Size = new System.Drawing.Size(100, 15);
            this.lblRotationAngle.TabIndex = 12;
            this.lblRotationAngle.Text = "rotation angle:";
            // 
            // txtBxAngleRotation
            // 
            this.txtBxAngleRotation.Location = new System.Drawing.Point(460, 43);
            this.txtBxAngleRotation.Name = "txtBxAngleRotation";
            this.txtBxAngleRotation.Size = new System.Drawing.Size(241, 20);
            this.txtBxAngleRotation.TabIndex = 13;
            this.txtBxAngleRotation.KeyDown += new System.Windows.Forms.KeyEventHandler(this.txtBxAngleRotation_KeyDown);
            // 
            // lblAnimation
            // 
            this.lblAnimation.AutoSize = true;
            this.lblAnimation.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lblAnimation.Location = new System.Drawing.Point(853, 247);
            this.lblAnimation.Name = "lblAnimation";
            this.lblAnimation.Size = new System.Drawing.Size(82, 18);
            this.lblAnimation.TabIndex = 14;
            this.lblAnimation.Text = "Animation";
            // 
            // pctBxAnimationObjectRotate
            // 
            this.pctBxAnimationObjectRotate.Location = new System.Drawing.Point(817, 84);
            this.pctBxAnimationObjectRotate.Name = "pctBxAnimationObjectRotate";
            this.pctBxAnimationObjectRotate.Size = new System.Drawing.Size(165, 137);
            this.pctBxAnimationObjectRotate.TabIndex = 15;
            this.pctBxAnimationObjectRotate.TabStop = false;
            // 
            // btnCreateAnimation
            // 
            this.btnCreateAnimation.Font = new System.Drawing.Font("Microsoft Sans Serif", 11.25F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.btnCreateAnimation.Location = new System.Drawing.Point(15, 216);
            this.btnCreateAnimation.Name = "btnCreateAnimation";
            this.btnCreateAnimation.Size = new System.Drawing.Size(252, 33);
            this.btnCreateAnimation.TabIndex = 16;
            this.btnCreateAnimation.Text = "Create Rotate Animation";
            this.btnCreateAnimation.UseVisualStyleBackColor = true;
            this.btnCreateAnimation.Click += new System.EventHandler(this.btnCreateAnimation_Click);
            // 
            // listView1
            // 
            this.listView1.HideSelection = false;
            this.listView1.Location = new System.Drawing.Point(856, 286);
            this.listView1.Name = "listView1";
            this.listView1.Size = new System.Drawing.Size(390, 120);
            this.listView1.TabIndex = 17;
            this.listView1.UseCompatibleStateImageBehavior = false;
            this.listView1.View = System.Windows.Forms.View.List;
            // 
            // lbPlaneRotate
            // 
            this.lbPlaneRotate.AutoSize = true;
            this.lbPlaneRotate.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lbPlaneRotate.Location = new System.Drawing.Point(12, 24);
            this.lbPlaneRotate.Name = "lbPlaneRotate";
            this.lbPlaneRotate.Size = new System.Drawing.Size(154, 16);
            this.lbPlaneRotate.TabIndex = 18;
            this.lbPlaneRotate.Text = "Choice Rotate Plane:";
            // 
            // lstBxInstructions
            // 
            this.lstBxInstructions.Font = new System.Drawing.Font("Microsoft Sans Serif", 9.75F, System.Drawing.FontStyle.Bold, System.Drawing.GraphicsUnit.Point, ((byte)(0)));
            this.lstBxInstructions.FormattingEnabled = true;
            this.lstBxInstructions.ItemHeight = 16;
            this.lstBxInstructions.Location = new System.Drawing.Point(12, 417);
            this.lstBxInstructions.Name = "lstBxInstructions";
            this.lstBxInstructions.Size = new System.Drawing.Size(1240, 164);
            this.lstBxInstructions.TabIndex = 19;
            // 
            // wndMain
            // 
            this.AutoScaleDimensions = new System.Drawing.SizeF(6F, 13F);
            this.AutoScaleMode = System.Windows.Forms.AutoScaleMode.Font;
            this.ClientSize = new System.Drawing.Size(1264, 587);
            this.Controls.Add(this.lstBxInstructions);
            this.Controls.Add(this.lbPlaneRotate);
            this.Controls.Add(this.listView1);
            this.Controls.Add(this.btnCreateAnimation);
            this.Controls.Add(this.pctBxAnimationObjectRotate);
            this.Controls.Add(this.lblAnimation);
            this.Controls.Add(this.txtBxAngleRotation);
            this.Controls.Add(this.lblRotationAngle);
            this.Controls.Add(this.cmbBxPlaneRotation);
            this.Controls.Add(this.btnCreateRotateImages);
            this.Controls.Add(this.pctBxImagesRotated);
            this.Controls.Add(this.lblImageOut);
            this.Controls.Add(this.lblImageIn);
            this.Controls.Add(this.pctBxImageOut);
            this.Controls.Add(this.pctBxImageIn);
            this.Controls.Add(this.label2);
            this.Controls.Add(this.BaseOrtonormalView);
            this.Controls.Add(this.menuStrip1);
            this.MainMenuStrip = this.menuStrip1;
            this.Name = "wndMain";
            this.Text = "Form1";
            this.Load += new System.EventHandler(this.wndMain_Load);
            this.menuStrip1.ResumeLayout(false);
            this.menuStrip1.PerformLayout();
            ((System.ComponentModel.ISupportInitialize)(this.pctBxImageIn)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBxImageOut)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBxImagesRotated)).EndInit();
            ((System.ComponentModel.ISupportInitialize)(this.pctBxAnimationObjectRotate)).EndInit();
            this.ResumeLayout(false);
            this.PerformLayout();

        }

        #endregion

        private System.Windows.Forms.MenuStrip menuStrip1;
        private System.Windows.Forms.ToolStripMenuItem filesToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem loadImageToolStripMenuItem;
        private System.Windows.Forms.ToolStripMenuItem saveImageProcessedToolStripMenuItem;
        private System.Windows.Forms.ListView BaseOrtonormalView;
        private System.Windows.Forms.Label label2;
        private System.Windows.Forms.PictureBox pctBxImageIn;
        private System.Windows.Forms.PictureBox pctBxImageOut;
        private System.Windows.Forms.Label lblImageIn;
        private System.Windows.Forms.Label lblImageOut;
        private System.Windows.Forms.PictureBox pctBxImagesRotated;
        private System.Windows.Forms.Button btnCreateRotateImages;
        private System.Windows.Forms.ComboBox cmbBxPlaneRotation;
        private System.Windows.Forms.Label lblRotationAngle;
        private System.Windows.Forms.TextBox txtBxAngleRotation;
        private System.Windows.Forms.Label lblAnimation;
        private System.Windows.Forms.PictureBox pctBxAnimationObjectRotate;
        private System.Windows.Forms.Button btnCreateAnimation;
        private System.Windows.Forms.ListView listView1;
        private System.Windows.Forms.Label lbPlaneRotate;
        private System.Windows.Forms.ListBox lstBxInstructions;
    }
}

