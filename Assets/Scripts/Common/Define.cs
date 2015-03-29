// 유틸성 함수 모음
public static class Util
{
    // 초단위 길이를 문자열로 변경
    public static string ConverBeatLength(int sec)
    {
        int lengthMin = sec / 60; // 분
        int lengthSec = sec % 60; // 초
        return (lengthMin.ToString() + ":" + lengthSec.ToString("00"));
    }
}