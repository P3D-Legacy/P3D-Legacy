// MonoGame - Copyright (C) The MonoGame Team
// This file is subject to the terms and conditions defined in
// file 'LICENSE.txt', which is part of this source code package.

using System;
using System.Collections.Generic;
using System.ComponentModel;
using Microsoft.Xna.Framework.Graphics;
using Microsoft.Xna.Framework.Content.Pipeline.Graphics;
using System.IO;

namespace Microsoft.Xna.Framework.Content.Pipeline.Processors
{

	/// <summary>
	/// Class to allow us to add the source path to the processor.
	/// </summary>
	public class LocalizedFontTextureContent : Texture2DContent
	{
		public List<char> mChars;
		public TextureContent mBaseContent;
		
		public LocalizedFontTextureContent(TextureContent baseContent, List<char> chars)
		{
			mChars = chars;
			mBaseContent = baseContent;
		}

		public override void Validate(GraphicsProfile? targetProf)
		{
			throw new NotImplementedException();
		}
	}

	/// <summary>
	/// Class to import a text file of chars along with the bitmap they reference
	/// </summary>
	[ContentImporter(".bmp", DefaultProcessor = "LocalizedFontTextureProcessor",
		 DisplayName = "Texture Importer - Localized Font")]
	class LocalizedFontTextureImporter : TextureImporter
	{
		public override TextureContent Import(string filename,
			ContentImporterContext context)
        {
            //Use the base monogame texture importer
            TextureContent baseTextureContent = base.Import(filename, context);

			//Get the text file that should have been generated with the same name as the bitmap
			string textFileName = Path.ChangeExtension(filename, ".txt");

			//Read all the characters
			string allChars = File.ReadAllText(textFileName);
			List<char> charList = new List<char>();

			// Scan each character of the string.
			foreach (char usedCharacter in allChars)
			{
				charList.Add(usedCharacter);
			}
			LocalizedFontTextureContent localizedFontTextureContent = new LocalizedFontTextureContent(baseTextureContent, charList);

			return localizedFontTextureContent;
		}
	}




	[ContentProcessorAttribute(DisplayName = "Font Texture - Localized")]
	public class LocalizedFontTextureProcessor : FontTextureProcessor
	{
		List<char> allCharacters;

		[DefaultValue('?')]
		public virtual char DefaultCharacter { get; set; }

		public LocalizedFontTextureProcessor() : base()
		{
			DefaultCharacter = '?';
		}

		/// <summary>
		/// Overridden to reference the list of characters.
		/// </summary>
		/// <param name="index"></param>
		/// <returns></returns>
		protected override char GetCharacterForIndex(int index)
		{
			return allCharacters[index];
		}
		

		public override SpriteFontContent Process(Texture2DContent input, ContentProcessorContext context)
		{
            //Cast the input to our own special format
            LocalizedFontTextureContent localizedContent = input as LocalizedFontTextureContent;


			//Our localized texture input just contains the base Texture2DContent and the list of characters
			Texture2DContent textureContent = localizedContent.mBaseContent as Texture2DContent;
			allCharacters = localizedContent.mChars;

			SpriteFontContent outputContent = base.Process(textureContent, context);

			//Need to set a default character for this kind of font as well
			outputContent.DefaultCharacter = DefaultCharacter;

			return outputContent;
		}
	}
}
