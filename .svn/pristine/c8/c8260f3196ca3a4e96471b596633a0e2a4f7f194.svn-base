using System;

namespace ViewModelLib
{
	public interface ISoundPlayer
	{
		/// <summary>
		/// Plays the sound located at soundFilePath (url).
		/// </summary>
		/// <param name="soundFilePath">the file path to the sound</param>
		/// <returns>True if sound started playing, false otherwise</returns>
		bool PlaySound(string soundFilePath);

		/// <summary>
		/// Plays the sound located at soundFilePath (url) as looping, doesn't stop until Stop() is invoked.
		/// </summary>
		/// <param name="filePath">True if the sound started playing, false otherwise</param>
		bool PlayLooping(string soundFilePath);

		/// <summary>
		/// Stop current playback (if any).
		/// </summary>
		void Stop();
	}
}
