﻿using System;
using System.Collections.Generic;
using System.Text;
using System.Drawing;
using System.Diagnostics;
using SlimDX;
using FDK;

namespace TJAPlayer3
{
	internal class CActResultSongBar : CActivity
	{
		// コンストラクタ

		public CActResultSongBar()
		{
			base.b活性化してない = true;
		}


		// メソッド

		public void tアニメを完了させる()
		{
			this.ct登場用.n現在の値 = this.ct登場用.n終了値;
		}


		// CActivity 実装

		public override void On活性化()
		{
            if( !string.IsNullOrEmpty( CDTXMania.ConfigIni.FontName) )
            {
                this.pfMusicName = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.FontName), CDTXMania.Skin.Result_MusicName_FontSize);
                this.pfStageText = new CPrivateFastFont(new FontFamily(CDTXMania.ConfigIni.FontName), CDTXMania.Skin.Result_StageText_FontSize);
            }
            else
            {
                this.pfMusicName = new CPrivateFastFont(new FontFamily("MS UI Gothic"), CDTXMania.Skin.Result_MusicName_FontSize);
                this.pfStageText = new CPrivateFastFont(new FontFamily("MS UI Gothic"), CDTXMania.Skin.Result_StageText_FontSize);
            }

		    // After performing calibration, inform the player that
		    // calibration has been completed, rather than
		    // displaying the song title as usual.


		    var title = CDTXMania.IsPerformingCalibration
		        ? $"Calibration complete. InputAdjustTime is now {CDTXMania.ConfigIni.nInputAdjustTimeMs}ms"
		        : CDTXMania.DTX.TITLE;

		    using (var bmpSongTitle = pfMusicName.DrawPrivateFont(title, CDTXMania.Skin.Result_MusicName_ForeColor, CDTXMania.Skin.Result_MusicName_BackColor))

		    {
		        this.txMusicName = CDTXMania.tテクスチャの生成(bmpSongTitle, false);
		        txMusicName.vc拡大縮小倍率.X = CDTXMania.GetSongNameXScaling(ref txMusicName);
		    }

		    using (var bmpStageText = pfStageText.DrawPrivateFont(CDTXMania.Skin.Game_StageText, CDTXMania.Skin.Result_StageText_ForeColor, CDTXMania.Skin.Result_StageText_BackColor))
		    {
		        this.txStageText = CDTXMania.tテクスチャの生成(bmpStageText, false);
		    }

			base.On活性化();
		}
		public override void On非活性化()
		{
			if( this.ct登場用 != null )
			{
				this.ct登場用 = null;
			}
			base.On非活性化();
		}
		public override void OnManagedリソースの作成()
		{
			if( !base.b活性化してない )
			{
				base.OnManagedリソースの作成();
			}
		}
		public override void OnManagedリソースの解放()
		{
			if( !base.b活性化してない )
			{
                CDTXMania.t安全にDisposeする(ref this.pfMusicName);
                CDTXMania.tテクスチャの解放( ref this.txMusicName );

                CDTXMania.t安全にDisposeする(ref this.pfStageText);
                CDTXMania.tテクスチャの解放(ref this.txStageText);
                base.OnManagedリソースの解放();
			}
		}
		public override int On進行描画()
		{
			if( base.b活性化してない )
			{
				return 0;
			}
			if( base.b初めての進行描画 )
			{
				this.ct登場用 = new CCounter( 0, 270, 4, CDTXMania.Timer );
				base.b初めての進行描画 = false;
			}
			this.ct登場用.t進行();

            if (CDTXMania.Skin.Result_MusicName_ReferencePoint == CSkin.ReferencePoint.Center)
            {
                this.txMusicName.t2D描画(CDTXMania.app.Device, CDTXMania.Skin.Result_MusicName_X - ((this.txMusicName.szテクスチャサイズ.Width * txMusicName.vc拡大縮小倍率.X) / 2), CDTXMania.Skin.Result_MusicName_Y);
            }
            else if (CDTXMania.Skin.Result_MusicName_ReferencePoint == CSkin.ReferencePoint.Left)
            {
                this.txMusicName.t2D描画(CDTXMania.app.Device, CDTXMania.Skin.Result_MusicName_X, CDTXMania.Skin.Result_MusicName_Y);
            }
            else
            {
                this.txMusicName.t2D描画(CDTXMania.app.Device, CDTXMania.Skin.Result_MusicName_X - this.txMusicName.szテクスチャサイズ.Width * txMusicName.vc拡大縮小倍率.X, CDTXMania.Skin.Result_MusicName_Y);
            }

            if(CDTXMania.stage選曲.n確定された曲の難易度 != (int)Difficulty.Dan)
            {
                if (CDTXMania.Skin.Result_StageText_ReferencePoint == CSkin.ReferencePoint.Center)
                {
                    this.txStageText.t2D描画(CDTXMania.app.Device, CDTXMania.Skin.Result_StageText_X - ((this.txStageText.szテクスチャサイズ.Width * txStageText.vc拡大縮小倍率.X) / 2), CDTXMania.Skin.Result_StageText_Y);
                }
                else if (CDTXMania.Skin.Result_StageText_ReferencePoint == CSkin.ReferencePoint.Right)
                {
                    this.txStageText.t2D描画(CDTXMania.app.Device, CDTXMania.Skin.Result_StageText_X - this.txStageText.szテクスチャサイズ.Width, CDTXMania.Skin.Result_StageText_Y);
                }
                else
                {
                    this.txStageText.t2D描画(CDTXMania.app.Device, CDTXMania.Skin.Result_StageText_X, CDTXMania.Skin.Result_StageText_Y);
                }
            }


			if( !this.ct登場用.b終了値に達した )
			{
				return 0;
			}
			return 1;
		}


		// その他

		#region [ private ]
		//-----------------
		private CCounter ct登場用;

        private CTexture txMusicName;
        private CPrivateFastFont pfMusicName;

        private CTexture txStageText;
        private CPrivateFont pfStageText;
        //-----------------
		#endregion
	}
}
