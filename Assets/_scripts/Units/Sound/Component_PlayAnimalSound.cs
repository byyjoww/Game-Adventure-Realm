using UnityEngine;

namespace PolyPerfect
{
    public class Component_PlayAnimalSound : MonoBehaviour
    {
        [SerializeField] private AudioSource audioSource;

        [SerializeField] private AudioClip animalSound;
        [SerializeField] private AudioClip walking;
        [SerializeField] private AudioClip eating;
        [SerializeField] private AudioClip running;
        [SerializeField] private AudioClip attacking;
        [SerializeField] private AudioClip death;
        [SerializeField] private AudioClip sleeping;

        public void AnimalSound()
        {
            if (animalSound) { audioSource.PlayOneShot(animalSound); }
        }

        public void Walking()
        {
            if (walking) { audioSource.PlayOneShot(walking); }
        }

        public void Eating()
        {
            if (eating) { audioSource.PlayOneShot(eating); }
        }

        public void Running()
        {
            if (running) { audioSource.PlayOneShot(running); }
        }

        public void Attacking()
        {
            if (attacking) { audioSource.PlayOneShot(attacking); }
        }

        public void Death()
        {
            if (death) { audioSource.PlayOneShot(death); }
        }

        public void Sleeping()
        {
            if (sleeping) { audioSource.PlayOneShot(sleeping); }
        }

        private void OnValidate()
        {
            if (audioSource == null) audioSource = GetComponent<AudioSource>();
        }
    }
}