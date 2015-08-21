using System;

namespace myMangaSiteExtension.Enums
{
    [Flags]
    public enum UserLoginType
    {
        None = 0x00,
        Username = 0x01,
        Token = 0x02,
        PaOtherge = 0x04
    }
}
