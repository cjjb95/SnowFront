using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Input;
using SkinnedModel;
using System;
using System.Collections.Generic;

namespace GDLibrary
{
    //used internally as a unique key (take + model name) to access a specific animation (useful when lots of FBX files have same default take name i.e. Take001)
    class AnimationDictionaryKey
    {
        public string takeName;
        public string fileNameNoSuffix;

        public AnimationDictionaryKey(string takeName, string fileNameNoSuffix)
        {
            this.takeName = takeName;
            this.fileNameNoSuffix = fileNameNoSuffix;
        }

        //Why do we override equals and gethashcode? Clue: this.modelDictionary.ContainsKey()
        public override bool Equals(object obj)
        {
            AnimationDictionaryKey other = obj as AnimationDictionaryKey;
            return this.takeName.Equals(other.takeName) && this.fileNameNoSuffix.Equals(other.fileNameNoSuffix);
        }

        public override int GetHashCode()
        {
            int hash = 1;
            hash = hash * 31 + this.takeName.GetHashCode();
            hash = hash * 17 + this.fileNameNoSuffix.GetHashCode();
            return hash;
        }

    }

    public class AnimatedPlayerObject : PlayerObject
    {
        #region Variables
        private AnimationStateType animationState;
        private AnimationPlayer animationPlayer;
        private SkinningData skinningData;
  
        //stores all the data related to a character with multiple individual FBX animation files (e.g. walk.fbx, idle,fbx, run.fbx)
        private Dictionary<AnimationDictionaryKey, Model> modelDictionary;
        private Dictionary<AnimationDictionaryKey, AnimationPlayer> animationPlayerDictionary;
        private Dictionary<AnimationDictionaryKey, SkinningData> skinningDataDictionary;
        private AnimationDictionaryKey oldKey;
        #endregion

        #region Properties
        public AnimationStateType AnimationState
        {
            get
            {
                return this.animationState;
            }
            set
            {
                this.animationState = value;
            }
        }
        public AnimationPlayer AnimationPlayer
        {
            get
            {
                return this.animationPlayer;
            }
        }
        #endregion

        public AnimatedPlayerObject(string id, ActorType actorType, Transform3D transform,
            EffectParameters effectParameters,
            Keys[] moveKeys, float radius, float height, 
            float accelerationRate, float decelerationRate, float jumpHeight,
            Vector3 translationOffset, KeyboardManager keyboardManager)
            : base(id, actorType, transform, effectParameters, null, moveKeys, radius, height, accelerationRate, decelerationRate, jumpHeight, translationOffset, keyboardManager)
        {
            //initialize dictionaries
            this.modelDictionary = new Dictionary<AnimationDictionaryKey, Model>();
            this.animationPlayerDictionary = new Dictionary<AnimationDictionaryKey, AnimationPlayer>();
            this.skinningDataDictionary = new Dictionary<AnimationDictionaryKey, SkinningData>();
        }

        public void AddAnimation(string takeName, string fileNameNoSuffix, Model model)
        {
            AnimationDictionaryKey key = new AnimationDictionaryKey(takeName, fileNameNoSuffix);

            //if not already added
            if (!this.modelDictionary.ContainsKey(key))
            {
                this.modelDictionary.Add(key, model);
                //read the skinning data (i.e. the set of transforms applied to each model bone for each frame of the animation)
                skinningData = model.Tag as SkinningData;

                if (skinningData == null)
                    throw new InvalidOperationException("The model [" + fileNameNoSuffix + "] does not contain a SkinningData tag.");

                //make an animation player for the model
                this.animationPlayerDictionary.Add(key, new AnimationPlayer(skinningData));

                //store the skinning data for the model 
                this.skinningDataDictionary.Add(key, skinningData);
            }
        }

        public override void Update(GameTime gameTime)
        {
            //update player to return bone transforms for the appropriate frame in the animation
            animationPlayer.Update(gameTime.ElapsedGameTime, true, Matrix.Identity);
            base.Update(gameTime);
        }

        //sets the first frame for the take and file (e.g. "Take 001", "dude")
        public void SetAnimation(string takeName, string fileNameNoSuffix)
        {
            AnimationDictionaryKey key = new AnimationDictionaryKey(takeName, fileNameNoSuffix);

            //have we requested a different animation and is it in the dictionary?
            //first time or different animation request
            if (this.oldKey == null || (!this.oldKey.Equals(key) && this.modelDictionary.ContainsKey(key)))
            {
                //set the model based on the animation being played
                this.Model = modelDictionary[key];

                //retrieve the animation player
                animationPlayer = animationPlayerDictionary[key];

                //retrieve the skinning data
                skinningData = skinningDataDictionary[key];

                //set the skinning data in the animation player and set the player to start at the first frame for the take
                animationPlayer.StartClip(skinningData.AnimationClips[key.takeName]);
            }


            //store current key for comparison in next update to prevent re-setting the same animation in successive calls to SetAnimation()
            this.oldKey = key;
        }

        //sets the take based on what the user presses/clicks
        protected virtual void SetAnimationByInput()
        {

        }
    }
}
