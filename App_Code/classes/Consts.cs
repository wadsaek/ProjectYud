using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

/// <summary>
/// Summary description for Consts
/// </summary>
public abstract class Consts
{
    public const string DEFAULT_PNG = "default.png";
    public const string DELETED_USERNAME = "(deleted user)";
    public const int    SONGS_ON_A_PAGE = 10;
    public const string VALID_SYMBOLS = "abcdefghijklmnopqrstuvwxyzABCDEFGHIJKLMNOPQRSTUVWXYZ1234567890@$";
    public const string SESSION_USER = "user";
    public const string adminPath = "../App_Data/admins.json";
    public const string DB_PATH_LOCAL = "../App_Data/database.mdf";
    public const string SESSION_LAST_PAGE = "LastPage";
    public const string IMAGES_DIR_LOCAL = "../images/pfps";
}