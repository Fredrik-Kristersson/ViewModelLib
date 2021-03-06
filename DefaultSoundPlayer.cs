﻿using System;
using System.IO;
using System.Media;
using WMPLib;

namespace ViewModelLib
{
	public class DefaultSoundPlayer : ISoundPlayer
	{
		WindowsMediaPlayer WMplayer = new WindowsMediaPlayer();
		SoundPlayer loopPlayer = new SoundPlayer();

		/// <summary>
		/// Plays the sound located at soundFilePath (url).
		/// </summary>
		/// <param name="soundFilePath">the file path to the sound</param>
		/// <returns>True if sound started playing, false otherwise</returns>
		public bool PlaySound(string soundFilePath)
		{
			Stop();
			if (!File.Exists(soundFilePath))
			{
				return false;
			}
			PlayWMA(soundFilePath);
			return true;
		}

		/// <summary>
		/// Plays the sound located at soundFilePath (url) as looping, doesn't stop until Stop() is invoked.
		/// </summary>
		/// <param name="filePath">True if the sound started playing, false otherwise</param>
		public bool PlayLooping(string soundFilePath)
		{
			Stop();
			if (!File.Exists(soundFilePath))
			{
				return false;
			}
			try
			{
				loopPlayer.SoundLocation = soundFilePath;
				loopPlayer.PlayLooping();
			}
			catch (Exception ex)
			{
				Console.Out.WriteLine("Couldn't play soundFile. " + ex.Message);
				return false;
			}
			return true;
		}

		/// <summary>
		/// Stop current playback (if any).
		/// </summary>
		public void Stop()
		{
			WMplayer.controls.stop();
			loopPlayer.Stop();
		}

		private void PlayWMA(string soundFilePath)
		{
			WMplayer.URL = soundFilePath;
			WMplayer.controls.play();
		}
	}
}
