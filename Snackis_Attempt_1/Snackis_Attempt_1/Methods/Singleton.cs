﻿namespace Snackis_Attempt_1.Methods
{
	public class Singleton
	{
		public static int PostId { get; set; }

		public static List<Models.Comment> PostComments { get; set; }

		public static string ProfilePic {  get; set; }

		public static void SetProfilePic(string profilePic)
		{
			ProfilePic = profilePic;
		}
		public static string GetProfilePic()
		{
			return ProfilePic;
		}
		public static void SetPostId(int postId)
		{
			PostId = postId;
		}
		public static int GetPostId()
		{
			return PostId;
		}

		public static void SetPostComments(List<Models.Comment> postComments)
		{
			PostComments = postComments;
		}

		public static List<Models.Comment> GetPostComments()
		{
			return PostComments;
		}
	}
}
