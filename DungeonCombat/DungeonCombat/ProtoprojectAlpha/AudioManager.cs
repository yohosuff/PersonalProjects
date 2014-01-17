using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using IrrKlang;


namespace DungeonCombat
{
    
    
    class AudioManager
    {
        static ISoundEngine soundEngine = null;
        public static List<SoundClip> soundClips = null;
        public static List<SoundClip> stepSounds = null;

        public static void Initialize()
        {
            soundEngine = new ISoundEngine();
            soundClips = new List<SoundClip>();
            stepSounds = new List<SoundClip>();
            
            LoadSoundClip(@"Sound\slash.wav", "slash");
            LoadSoundClip(@"Sound\death.wav", "death");
            LoadSoundClip(@"Sound\bow.wav", "bow");
            LoadSoundClip(@"Sound\death2.wav", "death2");
            LoadSoundClip(@"Sound\laugh.wav", "laugh");
            LoadSoundClip(@"Sound\punch.wav", "punch");

            LoadStepSound(@"Sound\steps\step0.wav");
            LoadStepSound(@"Sound\steps\step1.wav");
            LoadStepSound(@"Sound\steps\step2.wav");
            LoadStepSound(@"Sound\steps\step3.wav");
            LoadStepSound(@"Sound\steps\step4.wav");
            LoadStepSound(@"Sound\steps\step5.wav");
            LoadStepSound(@"Sound\steps\step6.wav");
            LoadStepSound(@"Sound\steps\step7.wav");
            LoadStepSound(@"Sound\steps\step8.wav");
            LoadStepSound(@"Sound\steps\step9.wav");
            LoadStepSound(@"Sound\steps\step10.wav");
            LoadStepSound(@"Sound\steps\step11.wav");
                        
        }

        public static void PlayRandomStepSound()
        {
            soundEngine.Play2D(stepSounds[Die.random.Next(stepSounds.Count())].path, false);
        }

        private static void LoadSoundClip(string path, string name)
        {
            soundClips.Add(new SoundClip(path, name));
        }
        
        private static void LoadStepSound(string path)
        {
            stepSounds.Add(new SoundClip(path, "step"));
        }
        
        public static void Play(string soundName)
        {
            SoundClip soundClip = soundClips.Find((SoundClip s) =>
                {
                    if (s.name == soundName)
                        return true;
                    return false;
                });

            soundEngine.Play2D(soundClip.path, false);

        }

    }

}
