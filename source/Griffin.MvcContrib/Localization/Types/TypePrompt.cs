﻿using System;
using System.Runtime.Serialization;
using System.Web;

namespace Griffin.MvcContrib.Localization.Types
{
    /// <summary>
    /// Used to store translated prompts.
    /// </summary>
    [DataContract]
    public class TypePrompt : IEquatable<TypePrompt>
    {
        private Type _subject;

        /// <summary>
        /// Initializes a new instance of the <see cref="TypePrompt"/> class.
        /// </summary>
        public TypePrompt()
        {
            UpdatedAt = DateTime.Now;
        }


        /// <summary>
        /// Initializes a new instance of the <see cref="TypePrompt"/> class.
        /// </summary>
        /// <param name="localeId">New locale </param>
        /// <param name="source">Copies all but translated text.</param>
        public TypePrompt(int localeId, TypePrompt source)
        {
            if (source == null) throw new ArgumentNullException("source");

            UpdatedAt = DateTime.Now;
            LocaleId = localeId;
            Subject = source.Subject;
            if (source.Subject == null)
                throw new ArgumentException("Subject is not specified.");
            Key = source.Key;
            TextName = source.TextName;
            TranslatedText = "";
        }


        /// <summary>
        /// Gets or sets target class
        /// </summary>
        /// <remarks>Might be a enum, validation attribute or model type</remarks>
        public Type Subject
        {
            get
            {
                if (_subject == null)
                {
                    if (SubjectTypeName == null)
                        throw new InvalidOperationException("Type has not been stored for " + TextName);

                    _subject = Type.GetType(SubjectTypeName);
                }

                return _subject;
            }
            set
            {
                _subject = value;
                SubjectTypeName = value.AssemblyQualifiedName;
            }
        }

        /// <summary>
        /// Gets or sets assembly qualified name
        /// </summary>
        [DataMember]
        public string SubjectTypeName { get; set; }

        /// <summary>
        /// Gets or sets text name
        /// </summary>
        /// <remarks>Might be a property name, an enum value name or just empty (actual type is being translated)</remarks>
        [DataMember]
        public string TextName { get; set; }

        /// <summary>
        /// Gets actual translation
        /// </summary>
        [DataMember]
        public string TranslatedText { get; set; }

        /// <summary>
        /// Gets when the prompt was updated.
        /// </summary>
        [DataMember]
        public DateTime UpdatedAt { get; set; }

        /// <summary>
        /// Ges or sets LCID identifier.
        /// </summary>
        [DataMember]
        public int LocaleId { get; set; }

        /// <summary>
        /// Gets or sets ID is unique for the current entry (in the current language)
        /// </summary>
        /// <remarks>The id should be the same in all languages</remarks>
        [DataMember]
        public TypePromptKey Key { get; set; }

        /// <summary>
        /// Gets or sets user id (using current identity <see cref="HttpContext.User"/>).
        /// </summary>
        public string UpdatedBy { get; set; }

        #region IEquatable<TypePrompt> Members

        /// <summary>
        /// Indicates whether the current object is equal to another object of the same type. (language not taken into account)
        /// </summary>
        /// <returns>
        /// true if the current object is equal to the <paramref name="other"/> parameter; otherwise, false.
        /// </returns>
        /// <param name="other">An object to compare with this object.</param>
        public bool Equals(TypePrompt other)
        {
            return other.Key.Equals(Key);
        }

        #endregion

        /// <summary>
        /// Returns a <see cref="System.String"/> that represents this instance.
        /// </summary>
        /// <returns>
        /// A <see cref="System.String"/> that represents this instance.
        /// </returns>
        public override string ToString()
        {
            return Subject.Name + "." + TextName + ": " + TranslatedText;
        }
    }
}