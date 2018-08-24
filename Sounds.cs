﻿using OpenBveApi.Runtime;
using System.Collections.Generic;

namespace MiscFunc {

    internal class Sounds {

        // --- クラス ---
        /// <summary>ループ音を表すクラス</summary>
        internal class Sound {
            internal readonly int Index;
            internal SoundHandle Handle;
            internal bool IsToBePlayed;

            internal Sound(int index) {
                this.Index = index;
                this.Handle = null;
            }

            internal void Play() {
                this.IsToBePlayed = true;
            }
        }

        // --- メンバ ---
        private readonly PlaySoundDelegate PlaySound;

        // --- 繰り返し再生 ---
        internal readonly Sound Eb;
        private readonly Sound[] LoopingSounds;

        // --- 1回再生 ---
        internal readonly Sound WiperSwDownSound;
        internal readonly Sound LcdSwDownSound;
        internal readonly Sound LcdSwUpSound;
        internal readonly Sound LightSwDownSound;
        internal readonly Sound LightSwUpSound;
        internal readonly Sound WiperSound;
        internal readonly Sound[] SwitchSound;
        private readonly List<Sound> PlayOnceSounds;

        // --- コンストラクタ ---
        /// <summary>新しいインスタンスを作成する</summary>
        /// <param name="playSound">サウンドを再生する関数のデリゲート。</param>
        internal Sounds(PlaySoundDelegate playSound) {
            this.PlaySound = playSound;

            // --- 繰り返し再生 ---
            this.Eb = new Sounds.Sound(13);
            this.LoopingSounds = new Sound[] { this.Eb };

            // --- 1回再生 ---
            this.WiperSwDownSound = new Sound(6);
            this.LcdSwDownSound = new Sound(61);
            this.LcdSwUpSound = new Sound(62);
            this.LightSwDownSound = new Sound(63);
            this.LightSwUpSound = new Sound(64);
            if (LoadConfig.WiperWet == 0) {
                this.WiperSound = new Sound(17);
            } else {
                this.WiperSound = new Sound(18);
            }
            this.SwitchSound = new Sound[LoadSwitch.ALL_SWITCH];
            for (int i = 0; i < LoadSwitch.switch_config_.Length; i++) {
                this.SwitchSound[i] = new Sound(LoadSwitch.switch_config_[i].switch_index_);
            }
            this.PlayOnceSounds = new List<Sound> { this.WiperSwDownSound, this.LcdSwDownSound, this.WiperSound };
            for (int i = 0; i < LoadSwitch.ALL_SWITCH; i++) {
                this.PlayOnceSounds.Add(this.SwitchSound[i]);
            }
        }

        // --- 関数 ---
        /// <summary>1フレームごとに呼び出される関数</summary>
        /// <param name="data">The data.</param>
        internal void Elapse(ElapseData data) {
            foreach (Sound sound in this.LoopingSounds) {
                if (sound.IsToBePlayed) {
                    if (sound.Handle == null || sound.Handle.Stopped) {
                        sound.Handle = PlaySound(sound.Index, 1.0, 1.0, true);
                    }
                } else {
                    if (sound.Handle != null && sound.Handle.Playing) {
                        sound.Handle.Stop();
                    }
                }
                sound.IsToBePlayed = false;
            }
            foreach (Sound sound in this.PlayOnceSounds) {
                if (sound.IsToBePlayed) {
                    PlaySound(sound.Index, 1.0, 1.0, false);
                    sound.IsToBePlayed = false;
                }
            }
        }
    }
}