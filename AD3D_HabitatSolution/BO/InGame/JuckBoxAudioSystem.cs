using AD3D_Common;
using SMLHelper.V2.Utility;
using System.Collections;
using System.Collections.Generic;
using System.IO;
using System.Reflection;
using UnityEngine;
using UnityEngine.UI;

namespace AD3D_HabitatSolutionMod.BO.InGame
{
    public class JuckBoxAudioSystem : MonoBehaviour
    {
        AudioSource AudioSource;
        List<AudioClip> AudioList = new List<AudioClip>();
        List<string> AudioListString = new List<string>();
        int currentTrack = 0;

        private Button btnPlay;
        private Button btnPause;
        private Button btnNextTrack;
        private Button btnPreviousTrack;
        private Button btnVolUp;
        private Button btnVolDown;
        private Text lblCurrentSong;
        private Text lblCurrentVol;

        public void Start()
        {
            AudioSource = GameObjectFinder.FindByName(this.gameObject, "Audio").GetComponent<AudioSource>();
            LoadAllSong();
            InitUI();

        }

        private void InitUI()
        {

            btnPlay = GameObjectFinder.FindByName(this.gameObject, "btnPlay").GetComponent<Button>();
            btnPlay.onClick.AddListener(() => Play());
            btnPause = GameObjectFinder.FindByName(this.gameObject, "btnPause").GetComponent<Button>();
            btnPause.onClick.AddListener(() => Pause());
            btnNextTrack = GameObjectFinder.FindByName(this.gameObject, "btnNextTrack").GetComponent<Button>();
            btnNextTrack.onClick.AddListener(() => SetTrack(1));
            btnPreviousTrack = GameObjectFinder.FindByName(this.gameObject, "btnPreviousTrack").GetComponent<Button>();
            btnPreviousTrack.onClick.AddListener(() => SetTrack(-1));
            btnVolUp = GameObjectFinder.FindByName(this.gameObject, "btnVolUp").GetComponent<Button>();
            btnVolUp.onClick.AddListener(() => SetAudio(0.05f));
            btnVolDown = GameObjectFinder.FindByName(this.gameObject, "btnVolDown").GetComponent<Button>();
            btnVolDown.onClick.AddListener(() => SetAudio(-0.05f));
            lblCurrentSong = GameObjectFinder.FindByName(this.gameObject, "lblCurrentSong").GetComponent<Text>();
            lblCurrentVol = GameObjectFinder.FindByName(this.gameObject, "lblCurrentVol").GetComponent<Text>();

            btnPlay.gameObject.SetActive(!AudioSource.isPlaying);
            btnPause.gameObject.SetActive(AudioSource.isPlaying);
            lblCurrentSong.text = AudioListString[currentTrack];
            lblCurrentVol.text = AudioSource.volume.ToString();
        }

        private void Update()
        {
            if(AudioSource.clip.samples == AudioSource.timeSamples)
                SetTrack(1);
        }

        private void Play()
        {
            AudioSource.clip = AudioList[currentTrack];
            lblCurrentSong.text = AudioListString[currentTrack];
            AudioSource.Play();

            btnPlay.gameObject.SetActive(!AudioSource.isPlaying);
            btnPause.gameObject.SetActive(AudioSource.isPlaying);

            AD3D_Common.Helper.Log($"Playing : n{currentTrack} {AudioListString[currentTrack]}", true);
        }

        private void Pause()
        {
            AudioSource.Pause();

            btnPlay.gameObject.SetActive(!AudioSource.isPlaying);
            btnPause.gameObject.SetActive(AudioSource.isPlaying);
        }

        private void SetTrack(int value)
        {
            if (currentTrack == AudioList.Count)
                currentTrack = 0;
            else if (currentTrack + value <= 0)
                currentTrack = AudioList.Count;
            else
                currentTrack += value;

            Play();
        }

        private void SetAudio(float value)
        {
            AudioSource.volume += value;
            lblCurrentVol.text = AudioSource.volume.ToString();
        }

        private void LoadAllSong()
        {
            var mainDirectory = Path.GetDirectoryName(Assembly.GetExecutingAssembly().Location);
            var assetsFolder = Path.Combine(mainDirectory, "Assets");
            DirectoryInfo d = new DirectoryInfo(assetsFolder);
            FileInfo[] Files = d.GetFiles("*.wav");
            //var path = @"D:/Steam/steamapps/common/Subnautica/QMods/AD3D_HabitatSolutionMod/Assets/ReggaetonMix.wav";
            foreach (var file in Files)
            {
                StartCoroutine(LoadSong(file.FullName));
            }
        }

        IEnumerator LoadSong(string filename)
        {
            WWW www = null;
            try
            {
                string url = $"file://{filename}";
                www = new WWW(url);
            }
            catch (System.Exception ex)
            {
                AD3D_Common.Helper.Log($"L : {ex}");
            }
            yield return www;
            var audioClip = www.GetAudioClip(false, false);
            AudioList.Add(audioClip);
            AudioListString.Add(Path.GetFileNameWithoutExtension(filename));
        }
    }
}
