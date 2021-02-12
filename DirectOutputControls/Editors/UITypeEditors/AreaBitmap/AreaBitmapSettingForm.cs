using DirectOutput.FX.MatrixFX;
using DirectOutput.General.BitmapHandling;
using System;
using System.Collections.Generic;
using System.ComponentModel;
using System.Data;
using System.Drawing;
using System.Drawing.Imaging;
using System.Linq;
using System.Text;
using System.Threading.Tasks;
using System.Windows.Forms;

namespace DirectOutputControls
{
    public partial class AreaBitmapSettingForm : Form
    {
        public AreaBitmapSetting Setting = null;

        private AreaBitmapSetting EditionSetting = new AreaBitmapSetting();

        public Image[] BitmapList { get; set; } = null;

        private Image CurrentImage = null;
        private FrameDimension CurrentDimension = null;
        private int FrameCount = 0;
        private Timer AnimationTimer = new Timer();
        private int CurrentAnimationFrame = 0;

        private Pen GridPen = new Pen(new SolidBrush(Color.Yellow));

        private float ZoomFactor = 1.0f;
        private int PixelSize = 1;
        private Point LastLocation = new Point();
        private Point Offset = new Point();
        private Rectangle ImageRect = new Rectangle();
        private Point StartDragLocation = new Point(-1, -1);
        private Rectangle SelectionRect = new Rectangle();

        public AreaBitmapSettingForm()
        {
            InitializeComponent();
        }

        private void AeraBitmapSettingForm_Load(object sender, EventArgs e)
        {
            comboBoxImageSelect.Items.Clear();
            comboBoxImageSelect.Items.AddRange(BitmapList.Select(I => I.Tag as string).ToArray());
            comboBoxImageSelect.SelectedIndex = 0;
            ComputeImageMetrics();
            EditionSetting = Setting.Clone() as AreaBitmapSetting;
            propertyGridSettings.SelectedObject = EditionSetting;
            SelectionRect = EditionSetting.Rect;
            AnimationTimer.Tick += AnimationTimer_Tick;
            UpdateTimer();
        }

        private void comboBoxImageSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            comboBoxFrameSelect.Items.Clear();
            CurrentImage = BitmapList[comboBoxImageSelect.SelectedIndex];

            CurrentDimension = new FrameDimension(CurrentImage.FrameDimensionsList[0]);
            FrameCount = CurrentImage.GetFrameCount(CurrentDimension);
            for(var num = 0; num < FrameCount; num++) {
                comboBoxFrameSelect.Items.Add($"Frame {num}");
            }
            comboBoxFrameSelect.SelectedIndex = 0;
        }

        private void comboBoxFrameSelect_SelectedIndexChanged(object sender, EventArgs e)
        {
            CurrentImage.SelectActiveFrame(CurrentDimension, comboBoxFrameSelect.SelectedIndex);
            EditionSetting.Frame = comboBoxFrameSelect.SelectedIndex;
        }

        private void ComputeImageMetrics()
        {
            var imageRatio = (float)CurrentImage.Width / CurrentImage.Height;
            if (imageRatio < 1.0f) {
                ImageRect.Height = (int)(CurrentImage.Height * (int)ZoomFactor);
                ImageRect.Width = (int)(ImageRect.Height * imageRatio);
            } else {
                ImageRect.Width = (int)(CurrentImage.Width * (int)ZoomFactor);
                ImageRect.Height = (int)(ImageRect.Width / imageRatio);
            }
            PixelSize = (ImageRect.Width / CurrentImage.Width).Limit(1, int.MaxValue);
            ImageRect.X = Offset.X + (pictureBoxImage.Width - ImageRect.Width) / 2;
            ImageRect.Y = Offset.Y + (pictureBoxImage.Height - ImageRect.Height) / 2;
        }

        private void pictureBoxImage_Paint(object sender, PaintEventArgs e)
        {
            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            e.Graphics.DrawRectangle(GridPen, ImageRect);
            e.Graphics.DrawImage(CurrentImage,ImageRect,0,0,CurrentImage.Width,CurrentImage.Height,GraphicsUnit.Pixel);

            var rectRatio = (float)ImageRect.Width / CurrentImage.Width;
            var posX = (int)((LastLocation.X - ImageRect.X) / rectRatio);
            var posY = (int)((LastLocation.Y - ImageRect.Y) / rectRatio);
            bool isDragging = !(StartDragLocation.X == -1 && StartDragLocation.Y == -1);
            if (!isDragging) {
                e.Graphics.FillRectangle(new SolidBrush(Color.White), new Rectangle(ImageRect.X + (posX * PixelSize), ImageRect.Y + (posY * PixelSize), PixelSize, PixelSize));
            } else {
                SelectionRect.X = (int)((StartDragLocation.X - ImageRect.X) / rectRatio);
                SelectionRect.Y = (int)((StartDragLocation.Y - ImageRect.Y) / rectRatio);
                SelectionRect.Width = posX - SelectionRect.X;
                SelectionRect.Height = posY - SelectionRect.Y;
            }

            if (SelectionRect.Width> 0 && SelectionRect.Height > 0){
                var halfPixelSize = PixelSize / 2;
                var frameRect = new Rectangle(ImageRect.X + (SelectionRect.X * PixelSize),
                                              ImageRect.Y + (SelectionRect.Y * PixelSize),
                                              SelectionRect.Width * PixelSize,
                                              SelectionRect.Height * PixelSize);
                if (isDragging) {
                    //Render main selection
                    e.Graphics.DrawRectangle(new Pen(new SolidBrush(Color.Red)), frameRect);
                } else {
                    //render animation frames
                    for (var num = 0; num < EditionSetting.AreaBitmapAnimationStepCount; num++) {
                        var animX = frameRect.X + (EditionSetting.AreaBitmapAnimationDirection == MatrixAnimationStepDirectionEnum.Right ? (num * EditionSetting.AreaBitmapAnimationStepSize * PixelSize) : 0);
                        var animY = frameRect.Y + (EditionSetting.AreaBitmapAnimationDirection == MatrixAnimationStepDirectionEnum.Down ? (num * EditionSetting.AreaBitmapAnimationStepSize * PixelSize) : 0);
                        e.Graphics.DrawRectangle(new Pen(new SolidBrush(num == CurrentAnimationFrame ? Color.Red : Color.Blue)) , new Rectangle(animX, animY, frameRect.Width, frameRect.Height));
                    }
                }
            }

            e.Graphics.FillRectangle(new SolidBrush(Color.FromArgb(64, Color.DarkBlue)), new Rectangle(0, 0, pictureBoxImage.Width, Math.Min(30, pictureBoxImage.Height)));
            e.Graphics.DrawString($"Zoom : {ZoomFactor}, Offset {Offset}, Rect {ImageRect}, LastLoc {LastLocation}, PixelSize {PixelSize}, Pos {posX},{posY}", Font, GridPen.Brush, new Point(10, 10));
        }

        private void pictureBoxPreview_Paint(object sender, PaintEventArgs e)
        {
            bool isDragging = !(StartDragLocation.X == -1 && StartDragLocation.Y == -1);
            if (isDragging) {
                return;
            }

            e.Graphics.InterpolationMode = System.Drawing.Drawing2D.InterpolationMode.NearestNeighbor;
            e.Graphics.PixelOffsetMode = System.Drawing.Drawing2D.PixelOffsetMode.Half;

            var frameRect = SelectionRect;
            switch (EditionSetting.AreaBitmapAnimationDirection) {
                case MatrixAnimationStepDirectionEnum.Down: {
                    frameRect.Y += CurrentAnimationFrame * EditionSetting.AreaBitmapAnimationStepSize;
                    break;
                }
                case MatrixAnimationStepDirectionEnum.Right: {
                    frameRect.X += CurrentAnimationFrame * EditionSetting.AreaBitmapAnimationStepSize;
                    break;
                }
                default:
                    break;
            }
            e.Graphics.DrawImage(CurrentImage, new Rectangle(0,0,SelectionRect.Width, SelectionRect.Height) 
                                , frameRect.X, frameRect.Y
                                , frameRect.Width, frameRect.Height, GraphicsUnit.Pixel);
        }

        private void AeraBitmapSettingForm_FormClosing(object sender, FormClosingEventArgs e)
        {
            AnimationTimer.Stop();
            CurrentImage.Dispose();
        }

        private void pictureBoxImage_MouseEnter(object sender, EventArgs e)
        {
            pictureBoxImage.MouseWheel += pictureBoxImage_MouseWheel;
        }

        private void pictureBoxImage_MouseWheel(object sender, MouseEventArgs e)
        {
            ZoomFactor += e.Delta * 0.01f;
            ZoomFactor = ZoomFactor.Limit(trackBarZoomFactor.Minimum, trackBarZoomFactor.Maximum);
            trackBarZoomFactor.Value = (int)ZoomFactor;
            ComputeImageMetrics();
            Refresh();
        }

        private void pictureBoxImage_MouseLeave(object sender, EventArgs e)
        {
            pictureBoxImage.MouseWheel -= pictureBoxImage_MouseWheel;
        }

        private void trackBarZoomFactor_Scroll(object sender, EventArgs e)
        {
            ZoomFactor = trackBarZoomFactor.Value;
            ComputeImageMetrics();
            Refresh();
        }

        private void pictureBoxImage_MouseMove(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Middle) {
                Offset.X += e.Location.X - LastLocation.X;
                Offset.Y += e.Location.Y - LastLocation.Y;
            }
            LastLocation = e.Location;
            ComputeImageMetrics();
            Refresh();
        }

        private void pictureBoxImage_MouseDown(object sender, MouseEventArgs e)
        {
            LastLocation = e.Location;
            if (e.Button == MouseButtons.Left) {
                StartDragLocation = e.Location;
            }
        }

        private void pictureBoxImage_MouseUp(object sender, MouseEventArgs e)
        {
            if (e.Button == MouseButtons.Left) {
                EditionSetting.Rect = SelectionRect;
                propertyGridSettings.Refresh();
                StartDragLocation = new Point(-1, -1);
            }
        }

        private void AeraBitmapSettingForm_Resize(object sender, EventArgs e)
        {
            ComputeImageMetrics();
            Refresh();
        }

        private void buttonValidate_Click(object sender, EventArgs e)
        {
            Setting = EditionSetting.Clone() as AreaBitmapSetting;
            Close();
        }

        private void UpdateTimer()
        {
            AnimationTimer.Stop();
            EditionSetting.AreaBitmapAnimationFrameRate = EditionSetting.AreaBitmapAnimationFrameRate.Limit(10, Int32.MaxValue);
            if (EditionSetting.AreaBitmapAnimationFrameRate > 0) {
                AnimationTimer.Interval = 1000 / EditionSetting.AreaBitmapAnimationFrameRate;
                AnimationTimer.Start();
            }
        }

        private void propertyGridSettings_PropertyValueChanged(object s, PropertyValueChangedEventArgs e)
        {
            switch (e.ChangedItem.PropertyDescriptor.Name) {
                case "AreaBitmapAnimationFrameRate":
                    UpdateTimer();
                    break;

                case "AreaBitmapAnimationDirection": {
                    if (EditionSetting.AreaBitmapAnimationDirection != MatrixAnimationStepDirectionEnum.Frame) {
                        CurrentImage?.SelectActiveFrame(CurrentDimension, comboBoxFrameSelect.SelectedIndex);
                    }
                    break;
                }

                default:
                    break;
            }
            if (e.ChangedItem.PropertyDescriptor.Name == "AreaBitmapAnimationFrameRate") {
                UpdateTimer();
            }
        }

        private void AnimationTimer_Tick(object sender, EventArgs e)
        {
            var maxFrame = EditionSetting.AreaBitmapAnimationDirection == MatrixAnimationStepDirectionEnum.Frame ? Math.Min(EditionSetting.AreaBitmapAnimationStepCount, FrameCount) : EditionSetting.AreaBitmapAnimationStepCount;
            if (maxFrame > 0) {
                CurrentAnimationFrame++;
                CurrentAnimationFrame = CurrentAnimationFrame % maxFrame;
                if (EditionSetting.AreaBitmapAnimationDirection == MatrixAnimationStepDirectionEnum.Frame) {
                    CurrentImage?.SelectActiveFrame(CurrentDimension, CurrentAnimationFrame);
                }
                Refresh();
            }
        }

    }
}
