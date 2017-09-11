using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace RunTecMs.Common
{
    public static class PinyinHelper
    {
        private static System.Collections.Hashtable _pinyinHash;

        static PinyinHelper()
        {
            PinyinHelper._pinyinHash = new System.Collections.Hashtable();
            PinyinHelper._pinyinHash.Add(-20319, "a");
            PinyinHelper._pinyinHash.Add(-20317, "ai");
            PinyinHelper._pinyinHash.Add(-20304, "an");
            PinyinHelper._pinyinHash.Add(-20295, "ang");
            PinyinHelper._pinyinHash.Add(-20292, "ao");
            PinyinHelper._pinyinHash.Add(-20283, "ba");
            PinyinHelper._pinyinHash.Add(-20265, "bai");
            PinyinHelper._pinyinHash.Add(-20257, "ban");
            PinyinHelper._pinyinHash.Add(-20242, "bang");
            PinyinHelper._pinyinHash.Add(-20230, "bao");
            PinyinHelper._pinyinHash.Add(-20051, "bei");
            PinyinHelper._pinyinHash.Add(-20036, "ben");
            PinyinHelper._pinyinHash.Add(-20032, "beng");
            PinyinHelper._pinyinHash.Add(-20026, "bi");
            PinyinHelper._pinyinHash.Add(-20002, "bian");
            PinyinHelper._pinyinHash.Add(-19990, "biao");
            PinyinHelper._pinyinHash.Add(-19986, "bie");
            PinyinHelper._pinyinHash.Add(-19982, "bin");
            PinyinHelper._pinyinHash.Add(-19976, "bing");
            PinyinHelper._pinyinHash.Add(-19805, "bo");
            PinyinHelper._pinyinHash.Add(-19784, "bu");
            PinyinHelper._pinyinHash.Add(-19775, "ca");
            PinyinHelper._pinyinHash.Add(-19774, "cai");
            PinyinHelper._pinyinHash.Add(-19763, "can");
            PinyinHelper._pinyinHash.Add(-19756, "cang");
            PinyinHelper._pinyinHash.Add(-19751, "cao");
            PinyinHelper._pinyinHash.Add(-19746, "ce");
            PinyinHelper._pinyinHash.Add(-19741, "ceng");
            PinyinHelper._pinyinHash.Add(-19739, "cha");
            PinyinHelper._pinyinHash.Add(-19728, "chai");
            PinyinHelper._pinyinHash.Add(-19725, "chan");
            PinyinHelper._pinyinHash.Add(-19715, "chang");
            PinyinHelper._pinyinHash.Add(-19540, "chao");
            PinyinHelper._pinyinHash.Add(-19531, "che");
            PinyinHelper._pinyinHash.Add(-19525, "chen");
            PinyinHelper._pinyinHash.Add(-19515, "cheng");
            PinyinHelper._pinyinHash.Add(-19500, "chi");
            PinyinHelper._pinyinHash.Add(-19484, "chong");
            PinyinHelper._pinyinHash.Add(-19479, "chou");
            PinyinHelper._pinyinHash.Add(-19467, "chu");
            PinyinHelper._pinyinHash.Add(-19289, "chuai");
            PinyinHelper._pinyinHash.Add(-19288, "chuan");
            PinyinHelper._pinyinHash.Add(-19281, "chuang");
            PinyinHelper._pinyinHash.Add(-19275, "chui");
            PinyinHelper._pinyinHash.Add(-19270, "chun");
            PinyinHelper._pinyinHash.Add(-19263, "chuo");
            PinyinHelper._pinyinHash.Add(-19261, "ci");
            PinyinHelper._pinyinHash.Add(-19249, "cong");
            PinyinHelper._pinyinHash.Add(-19243, "cou");
            PinyinHelper._pinyinHash.Add(-19242, "cu");
            PinyinHelper._pinyinHash.Add(-19238, "cuan");
            PinyinHelper._pinyinHash.Add(-19235, "cui");
            PinyinHelper._pinyinHash.Add(-19227, "cun");
            PinyinHelper._pinyinHash.Add(-19224, "cuo");
            PinyinHelper._pinyinHash.Add(-19218, "da");
            PinyinHelper._pinyinHash.Add(-19212, "dai");
            PinyinHelper._pinyinHash.Add(-19038, "dan");
            PinyinHelper._pinyinHash.Add(-19023, "dang");
            PinyinHelper._pinyinHash.Add(-19018, "dao");
            PinyinHelper._pinyinHash.Add(-19006, "de");
            PinyinHelper._pinyinHash.Add(-19003, "deng");
            PinyinHelper._pinyinHash.Add(-18996, "di");
            PinyinHelper._pinyinHash.Add(-18977, "dian");
            PinyinHelper._pinyinHash.Add(-18961, "diao");
            PinyinHelper._pinyinHash.Add(-18952, "die");
            PinyinHelper._pinyinHash.Add(-18783, "ding");
            PinyinHelper._pinyinHash.Add(-18774, "diu");
            PinyinHelper._pinyinHash.Add(-18773, "dong");
            PinyinHelper._pinyinHash.Add(-18763, "dou");
            PinyinHelper._pinyinHash.Add(-18756, "du");
            PinyinHelper._pinyinHash.Add(-18741, "duan");
            PinyinHelper._pinyinHash.Add(-18735, "dui");
            PinyinHelper._pinyinHash.Add(-18731, "dun");
            PinyinHelper._pinyinHash.Add(-18722, "duo");
            PinyinHelper._pinyinHash.Add(-18710, "e");
            PinyinHelper._pinyinHash.Add(-18697, "en");
            PinyinHelper._pinyinHash.Add(-18696, "er");
            PinyinHelper._pinyinHash.Add(-18526, "fa");
            PinyinHelper._pinyinHash.Add(-18518, "fan");
            PinyinHelper._pinyinHash.Add(-18501, "fang");
            PinyinHelper._pinyinHash.Add(-18490, "fei");
            PinyinHelper._pinyinHash.Add(-18478, "fen");
            PinyinHelper._pinyinHash.Add(-18463, "feng");
            PinyinHelper._pinyinHash.Add(-18448, "fo");
            PinyinHelper._pinyinHash.Add(-18447, "fou");
            PinyinHelper._pinyinHash.Add(-18446, "fu");
            PinyinHelper._pinyinHash.Add(-18239, "ga");
            PinyinHelper._pinyinHash.Add(-18237, "gai");
            PinyinHelper._pinyinHash.Add(-18231, "gan");
            PinyinHelper._pinyinHash.Add(-18220, "gang");
            PinyinHelper._pinyinHash.Add(-18211, "gao");
            PinyinHelper._pinyinHash.Add(-18201, "ge");
            PinyinHelper._pinyinHash.Add(-18184, "gei");
            PinyinHelper._pinyinHash.Add(-18183, "gen");
            PinyinHelper._pinyinHash.Add(-18181, "geng");
            PinyinHelper._pinyinHash.Add(-18012, "gong");
            PinyinHelper._pinyinHash.Add(-17997, "gou");
            PinyinHelper._pinyinHash.Add(-17988, "gu");
            PinyinHelper._pinyinHash.Add(-17970, "gua");
            PinyinHelper._pinyinHash.Add(-17964, "guai");
            PinyinHelper._pinyinHash.Add(-17961, "guan");
            PinyinHelper._pinyinHash.Add(-17950, "guang");
            PinyinHelper._pinyinHash.Add(-17947, "gui");
            PinyinHelper._pinyinHash.Add(-17931, "gun");
            PinyinHelper._pinyinHash.Add(-17928, "guo");
            PinyinHelper._pinyinHash.Add(-17922, "ha");
            PinyinHelper._pinyinHash.Add(-17759, "hai");
            PinyinHelper._pinyinHash.Add(-17752, "han");
            PinyinHelper._pinyinHash.Add(-17733, "hang");
            PinyinHelper._pinyinHash.Add(-17730, "hao");
            PinyinHelper._pinyinHash.Add(-17721, "he");
            PinyinHelper._pinyinHash.Add(-17703, "hei");
            PinyinHelper._pinyinHash.Add(-17701, "hen");
            PinyinHelper._pinyinHash.Add(-17697, "heng");
            PinyinHelper._pinyinHash.Add(-17692, "hong");
            PinyinHelper._pinyinHash.Add(-17683, "hou");
            PinyinHelper._pinyinHash.Add(-17676, "hu");
            PinyinHelper._pinyinHash.Add(-17496, "hua");
            PinyinHelper._pinyinHash.Add(-17487, "huai");
            PinyinHelper._pinyinHash.Add(-17482, "huan");
            PinyinHelper._pinyinHash.Add(-17468, "huang");
            PinyinHelper._pinyinHash.Add(-17454, "hui");
            PinyinHelper._pinyinHash.Add(-17433, "hun");
            PinyinHelper._pinyinHash.Add(-17427, "huo");
            PinyinHelper._pinyinHash.Add(-17417, "ji");
            PinyinHelper._pinyinHash.Add(-17202, "jia");
            PinyinHelper._pinyinHash.Add(-17185, "jian");
            PinyinHelper._pinyinHash.Add(-16983, "jiang");
            PinyinHelper._pinyinHash.Add(-16970, "jiao");
            PinyinHelper._pinyinHash.Add(-16942, "jie");
            PinyinHelper._pinyinHash.Add(-16915, "jin");
            PinyinHelper._pinyinHash.Add(-16733, "jing");
            PinyinHelper._pinyinHash.Add(-16708, "jiong");
            PinyinHelper._pinyinHash.Add(-16706, "jiu");
            PinyinHelper._pinyinHash.Add(-16689, "ju");
            PinyinHelper._pinyinHash.Add(-16664, "juan");
            PinyinHelper._pinyinHash.Add(-16657, "jue");
            PinyinHelper._pinyinHash.Add(-16647, "jun");
            PinyinHelper._pinyinHash.Add(-16474, "ka");
            PinyinHelper._pinyinHash.Add(-16470, "kai");
            PinyinHelper._pinyinHash.Add(-16465, "kan");
            PinyinHelper._pinyinHash.Add(-16459, "kang");
            PinyinHelper._pinyinHash.Add(-16452, "kao");
            PinyinHelper._pinyinHash.Add(-16448, "ke");
            PinyinHelper._pinyinHash.Add(-16433, "ken");
            PinyinHelper._pinyinHash.Add(-16429, "keng");
            PinyinHelper._pinyinHash.Add(-16427, "kong");
            PinyinHelper._pinyinHash.Add(-16423, "kou");
            PinyinHelper._pinyinHash.Add(-16419, "ku");
            PinyinHelper._pinyinHash.Add(-16412, "kua");
            PinyinHelper._pinyinHash.Add(-16407, "kuai");
            PinyinHelper._pinyinHash.Add(-16403, "kuan");
            PinyinHelper._pinyinHash.Add(-16401, "kuang");
            PinyinHelper._pinyinHash.Add(-16393, "kui");
            PinyinHelper._pinyinHash.Add(-16220, "kun");
            PinyinHelper._pinyinHash.Add(-16216, "kuo");
            PinyinHelper._pinyinHash.Add(-16212, "la");
            PinyinHelper._pinyinHash.Add(-16205, "lai");
            PinyinHelper._pinyinHash.Add(-16202, "lan");
            PinyinHelper._pinyinHash.Add(-16187, "lang");
            PinyinHelper._pinyinHash.Add(-16180, "lao");
            PinyinHelper._pinyinHash.Add(-16171, "le");
            PinyinHelper._pinyinHash.Add(-16169, "lei");
            PinyinHelper._pinyinHash.Add(-16158, "leng");
            PinyinHelper._pinyinHash.Add(-16155, "li");
            PinyinHelper._pinyinHash.Add(-15959, "lia");
            PinyinHelper._pinyinHash.Add(-15958, "lian");
            PinyinHelper._pinyinHash.Add(-15944, "liang");
            PinyinHelper._pinyinHash.Add(-15933, "liao");
            PinyinHelper._pinyinHash.Add(-15920, "lie");
            PinyinHelper._pinyinHash.Add(-15915, "lin");
            PinyinHelper._pinyinHash.Add(-15903, "ling");
            PinyinHelper._pinyinHash.Add(-15889, "liu");
            PinyinHelper._pinyinHash.Add(-15878, "long");
            PinyinHelper._pinyinHash.Add(-15707, "lou");
            PinyinHelper._pinyinHash.Add(-15701, "lu");
            PinyinHelper._pinyinHash.Add(-15681, "lv");
            PinyinHelper._pinyinHash.Add(-15667, "luan");
            PinyinHelper._pinyinHash.Add(-15661, "lue");
            PinyinHelper._pinyinHash.Add(-15659, "lun");
            PinyinHelper._pinyinHash.Add(-15652, "luo");
            PinyinHelper._pinyinHash.Add(-15640, "ma");
            PinyinHelper._pinyinHash.Add(-15631, "mai");
            PinyinHelper._pinyinHash.Add(-15625, "man");
            PinyinHelper._pinyinHash.Add(-15454, "mang");
            PinyinHelper._pinyinHash.Add(-15448, "mao");
            PinyinHelper._pinyinHash.Add(-15436, "me");
            PinyinHelper._pinyinHash.Add(-15435, "mei");
            PinyinHelper._pinyinHash.Add(-15419, "men");
            PinyinHelper._pinyinHash.Add(-15416, "meng");
            PinyinHelper._pinyinHash.Add(-15408, "mi");
            PinyinHelper._pinyinHash.Add(-15394, "mian");
            PinyinHelper._pinyinHash.Add(-15385, "miao");
            PinyinHelper._pinyinHash.Add(-15377, "mie");
            PinyinHelper._pinyinHash.Add(-15375, "min");
            PinyinHelper._pinyinHash.Add(-15369, "ming");
            PinyinHelper._pinyinHash.Add(-15363, "miu");
            PinyinHelper._pinyinHash.Add(-15362, "mo");
            PinyinHelper._pinyinHash.Add(-15183, "mou");
            PinyinHelper._pinyinHash.Add(-15180, "mu");
            PinyinHelper._pinyinHash.Add(-15165, "na");
            PinyinHelper._pinyinHash.Add(-15158, "nai");
            PinyinHelper._pinyinHash.Add(-15153, "nan");
            PinyinHelper._pinyinHash.Add(-15150, "nang");
            PinyinHelper._pinyinHash.Add(-15149, "nao");
            PinyinHelper._pinyinHash.Add(-15144, "ne");
            PinyinHelper._pinyinHash.Add(-15143, "nei");
            PinyinHelper._pinyinHash.Add(-15141, "nen");
            PinyinHelper._pinyinHash.Add(-15140, "neng");
            PinyinHelper._pinyinHash.Add(-15139, "ni");
            PinyinHelper._pinyinHash.Add(-15128, "nian");
            PinyinHelper._pinyinHash.Add(-15121, "niang");
            PinyinHelper._pinyinHash.Add(-15119, "niao");
            PinyinHelper._pinyinHash.Add(-15117, "nie");
            PinyinHelper._pinyinHash.Add(-15110, "nin");
            PinyinHelper._pinyinHash.Add(-15109, "ning");
            PinyinHelper._pinyinHash.Add(-14941, "niu");
            PinyinHelper._pinyinHash.Add(-14937, "nong");
            PinyinHelper._pinyinHash.Add(-14933, "nu");
            PinyinHelper._pinyinHash.Add(-14930, "nv");
            PinyinHelper._pinyinHash.Add(-14929, "nuan");
            PinyinHelper._pinyinHash.Add(-14928, "nue");
            PinyinHelper._pinyinHash.Add(-14926, "nuo");
            PinyinHelper._pinyinHash.Add(-14922, "o");
            PinyinHelper._pinyinHash.Add(-14921, "ou");
            PinyinHelper._pinyinHash.Add(-14914, "pa");
            PinyinHelper._pinyinHash.Add(-14908, "pai");
            PinyinHelper._pinyinHash.Add(-14902, "pan");
            PinyinHelper._pinyinHash.Add(-14894, "pang");
            PinyinHelper._pinyinHash.Add(-14889, "pao");
            PinyinHelper._pinyinHash.Add(-14882, "pei");
            PinyinHelper._pinyinHash.Add(-14873, "pen");
            PinyinHelper._pinyinHash.Add(-14871, "peng");
            PinyinHelper._pinyinHash.Add(-14857, "pi");
            PinyinHelper._pinyinHash.Add(-14678, "pian");
            PinyinHelper._pinyinHash.Add(-14674, "piao");
            PinyinHelper._pinyinHash.Add(-14670, "pie");
            PinyinHelper._pinyinHash.Add(-14668, "pin");
            PinyinHelper._pinyinHash.Add(-14663, "ping");
            PinyinHelper._pinyinHash.Add(-14654, "po");
            PinyinHelper._pinyinHash.Add(-14645, "pu");
            PinyinHelper._pinyinHash.Add(-14630, "qi");
            PinyinHelper._pinyinHash.Add(-14594, "qia");
            PinyinHelper._pinyinHash.Add(-14429, "qian");
            PinyinHelper._pinyinHash.Add(-14407, "qiang");
            PinyinHelper._pinyinHash.Add(-14399, "qiao");
            PinyinHelper._pinyinHash.Add(-14384, "qie");
            PinyinHelper._pinyinHash.Add(-14379, "qin");
            PinyinHelper._pinyinHash.Add(-14368, "qing");
            PinyinHelper._pinyinHash.Add(-14355, "qiong");
            PinyinHelper._pinyinHash.Add(-14353, "qiu");
            PinyinHelper._pinyinHash.Add(-14345, "qu");
            PinyinHelper._pinyinHash.Add(-14170, "quan");
            PinyinHelper._pinyinHash.Add(-14159, "que");
            PinyinHelper._pinyinHash.Add(-14151, "qun");
            PinyinHelper._pinyinHash.Add(-14149, "ran");
            PinyinHelper._pinyinHash.Add(-14145, "rang");
            PinyinHelper._pinyinHash.Add(-14140, "rao");
            PinyinHelper._pinyinHash.Add(-14137, "re");
            PinyinHelper._pinyinHash.Add(-14135, "ren");
            PinyinHelper._pinyinHash.Add(-14125, "reng");
            PinyinHelper._pinyinHash.Add(-14123, "ri");
            PinyinHelper._pinyinHash.Add(-14122, "rong");
            PinyinHelper._pinyinHash.Add(-14112, "rou");
            PinyinHelper._pinyinHash.Add(-14109, "ru");
            PinyinHelper._pinyinHash.Add(-14099, "ruan");
            PinyinHelper._pinyinHash.Add(-14097, "rui");
            PinyinHelper._pinyinHash.Add(-14094, "run");
            PinyinHelper._pinyinHash.Add(-14092, "ruo");
            PinyinHelper._pinyinHash.Add(-14090, "sa");
            PinyinHelper._pinyinHash.Add(-14087, "sai");
            PinyinHelper._pinyinHash.Add(-14083, "san");
            PinyinHelper._pinyinHash.Add(-13917, "sang");
            PinyinHelper._pinyinHash.Add(-13914, "sao");
            PinyinHelper._pinyinHash.Add(-13910, "se");
            PinyinHelper._pinyinHash.Add(-13907, "sen");
            PinyinHelper._pinyinHash.Add(-13906, "seng");
            PinyinHelper._pinyinHash.Add(-13905, "sha");
            PinyinHelper._pinyinHash.Add(-13896, "shai");
            PinyinHelper._pinyinHash.Add(-13894, "shan");
            PinyinHelper._pinyinHash.Add(-13878, "shang");
            PinyinHelper._pinyinHash.Add(-13870, "shao");
            PinyinHelper._pinyinHash.Add(-13859, "she");
            PinyinHelper._pinyinHash.Add(-13847, "shen");
            PinyinHelper._pinyinHash.Add(-13831, "sheng");
            PinyinHelper._pinyinHash.Add(-13658, "shi");
            PinyinHelper._pinyinHash.Add(-13611, "shou");
            PinyinHelper._pinyinHash.Add(-13601, "shu");
            PinyinHelper._pinyinHash.Add(-13406, "shua");
            PinyinHelper._pinyinHash.Add(-13404, "shuai");
            PinyinHelper._pinyinHash.Add(-13400, "shuan");
            PinyinHelper._pinyinHash.Add(-13398, "shuang");
            PinyinHelper._pinyinHash.Add(-13395, "shui");
            PinyinHelper._pinyinHash.Add(-13391, "shun");
            PinyinHelper._pinyinHash.Add(-13387, "shuo");
            PinyinHelper._pinyinHash.Add(-13383, "si");
            PinyinHelper._pinyinHash.Add(-13367, "song");
            PinyinHelper._pinyinHash.Add(-13359, "sou");
            PinyinHelper._pinyinHash.Add(-13356, "su");
            PinyinHelper._pinyinHash.Add(-13343, "suan");
            PinyinHelper._pinyinHash.Add(-13340, "sui");
            PinyinHelper._pinyinHash.Add(-13329, "sun");
            PinyinHelper._pinyinHash.Add(-13326, "suo");
            PinyinHelper._pinyinHash.Add(-13318, "ta");
            PinyinHelper._pinyinHash.Add(-13147, "tai");
            PinyinHelper._pinyinHash.Add(-13138, "tan");
            PinyinHelper._pinyinHash.Add(-13120, "tang");
            PinyinHelper._pinyinHash.Add(-13107, "tao");
            PinyinHelper._pinyinHash.Add(-13096, "te");
            PinyinHelper._pinyinHash.Add(-13095, "teng");
            PinyinHelper._pinyinHash.Add(-13091, "ti");
            PinyinHelper._pinyinHash.Add(-13076, "tian");
            PinyinHelper._pinyinHash.Add(-13068, "tiao");
            PinyinHelper._pinyinHash.Add(-13063, "tie");
            PinyinHelper._pinyinHash.Add(-13060, "ting");
            PinyinHelper._pinyinHash.Add(-12888, "tong");
            PinyinHelper._pinyinHash.Add(-12875, "tou");
            PinyinHelper._pinyinHash.Add(-12871, "tu");
            PinyinHelper._pinyinHash.Add(-12860, "tuan");
            PinyinHelper._pinyinHash.Add(-12858, "tui");
            PinyinHelper._pinyinHash.Add(-12852, "tun");
            PinyinHelper._pinyinHash.Add(-12849, "tuo");
            PinyinHelper._pinyinHash.Add(-12838, "wa");
            PinyinHelper._pinyinHash.Add(-12831, "wai");
            PinyinHelper._pinyinHash.Add(-12829, "wan");
            PinyinHelper._pinyinHash.Add(-12812, "wang");
            PinyinHelper._pinyinHash.Add(-12802, "wei");
            PinyinHelper._pinyinHash.Add(-12607, "wen");
            PinyinHelper._pinyinHash.Add(-12597, "weng");
            PinyinHelper._pinyinHash.Add(-12594, "wo");
            PinyinHelper._pinyinHash.Add(-12585, "wu");
            PinyinHelper._pinyinHash.Add(-12556, "xi");
            PinyinHelper._pinyinHash.Add(-12359, "xia");
            PinyinHelper._pinyinHash.Add(-12346, "xian");
            PinyinHelper._pinyinHash.Add(-12320, "xiang");
            PinyinHelper._pinyinHash.Add(-12300, "xiao");
            PinyinHelper._pinyinHash.Add(-12120, "xie");
            PinyinHelper._pinyinHash.Add(-12099, "xin");
            PinyinHelper._pinyinHash.Add(-12089, "xing");
            PinyinHelper._pinyinHash.Add(-12074, "xiong");
            PinyinHelper._pinyinHash.Add(-12067, "xiu");
            PinyinHelper._pinyinHash.Add(-12058, "xu");
            PinyinHelper._pinyinHash.Add(-12039, "xuan");
            PinyinHelper._pinyinHash.Add(-11867, "xue");
            PinyinHelper._pinyinHash.Add(-11861, "xun");
            PinyinHelper._pinyinHash.Add(-11847, "ya");
            PinyinHelper._pinyinHash.Add(-11831, "yan");
            PinyinHelper._pinyinHash.Add(-11798, "yang");
            PinyinHelper._pinyinHash.Add(-11781, "yao");
            PinyinHelper._pinyinHash.Add(-11604, "ye");
            PinyinHelper._pinyinHash.Add(-11589, "yi");
            PinyinHelper._pinyinHash.Add(-11536, "yin");
            PinyinHelper._pinyinHash.Add(-11358, "ying");
            PinyinHelper._pinyinHash.Add(-11340, "yo");
            PinyinHelper._pinyinHash.Add(-11339, "yong");
            PinyinHelper._pinyinHash.Add(-11324, "you");
            PinyinHelper._pinyinHash.Add(-11303, "yu");
            PinyinHelper._pinyinHash.Add(-11097, "yuan");
            PinyinHelper._pinyinHash.Add(-11077, "yue");
            PinyinHelper._pinyinHash.Add(-11067, "yun");
            PinyinHelper._pinyinHash.Add(-11055, "za");
            PinyinHelper._pinyinHash.Add(-11052, "zai");
            PinyinHelper._pinyinHash.Add(-11045, "zan");
            PinyinHelper._pinyinHash.Add(-11041, "zang");
            PinyinHelper._pinyinHash.Add(-11038, "zao");
            PinyinHelper._pinyinHash.Add(-11024, "ze");
            PinyinHelper._pinyinHash.Add(-11020, "zei");
            PinyinHelper._pinyinHash.Add(-11019, "zen");
            PinyinHelper._pinyinHash.Add(-11018, "zeng");
            PinyinHelper._pinyinHash.Add(-11014, "zha");
            PinyinHelper._pinyinHash.Add(-10838, "zhai");
            PinyinHelper._pinyinHash.Add(-10832, "zhan");
            PinyinHelper._pinyinHash.Add(-10815, "zhang");
            PinyinHelper._pinyinHash.Add(-10800, "zhao");
            PinyinHelper._pinyinHash.Add(-10790, "zhe");
            PinyinHelper._pinyinHash.Add(-10780, "zhen");
            PinyinHelper._pinyinHash.Add(-10764, "zheng");
            PinyinHelper._pinyinHash.Add(-10587, "zhi");
            PinyinHelper._pinyinHash.Add(-10544, "zhong");
            PinyinHelper._pinyinHash.Add(-10533, "zhou");
            PinyinHelper._pinyinHash.Add(-10519, "zhu");
            PinyinHelper._pinyinHash.Add(-10331, "zhua");
            PinyinHelper._pinyinHash.Add(-10329, "zhuai");
            PinyinHelper._pinyinHash.Add(-10328, "zhuan");
            PinyinHelper._pinyinHash.Add(-10322, "zhuang");
            PinyinHelper._pinyinHash.Add(-10315, "zhui");
            PinyinHelper._pinyinHash.Add(-10309, "zhun");
            PinyinHelper._pinyinHash.Add(-10307, "zhuo");
            PinyinHelper._pinyinHash.Add(-10296, "zi");
            PinyinHelper._pinyinHash.Add(-10281, "zong");
            PinyinHelper._pinyinHash.Add(-10274, "zou");
            PinyinHelper._pinyinHash.Add(-10270, "zu");
            PinyinHelper._pinyinHash.Add(-10262, "zuan");
            PinyinHelper._pinyinHash.Add(-10260, "zui");
            PinyinHelper._pinyinHash.Add(-10256, "zun");
            PinyinHelper._pinyinHash.Add(-10254, "zuo");
            PinyinHelper._pinyinHash.Add(-10247, "zz");
        }

        public static string GetPinyin(string chineseChars)
        {
            byte[] bytes = System.Text.Encoding.Default.GetBytes(chineseChars);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(chineseChars.Length * 4);
            for (int i = 0; i < bytes.Length; i++)
            {
                int num = (int)bytes[i];
                if (num > 160)
                {
                    i++;
                    if (i < bytes.Length)
                    {
                        num = num * 256 + (int)bytes[i] - 65536;
                    }
                    stringBuilder.Append(PinyinHelper.GetPinyin(num));
                }
                else if ((num >= 48 && num <= 57) || (num >= 65 && num <= 90) || (num >= 97 && num <= 122))
                {
                    stringBuilder.Append((char)num);
                }
                else
                {
                    stringBuilder.Append('-');
                }
            }
            string text = stringBuilder.ToString().Replace("--", "-").Replace("--", "-").ToLower();
            if (text.Replace("--", "-").LastIndexOf("-") == text.Replace("--", "-").Length - 1)
            {
                if (text.LastIndexOf("-") != -1)
                {
                    text = text.Substring(0, text.LastIndexOf("-"));
                }
            }
            return text;
        }

        public static string GetShortPinyin(string chineseChars)
        {
            chineseChars = StringPlus.SubString(chineseChars.Replace("'", "").Replace("‘", "").Replace("\"", "").Replace("“", "").Replace("”", "").Replace(" ", "").Replace(",", "").Replace("，", ""), 15, null);
            byte[] bytes = System.Text.Encoding.Default.GetBytes(chineseChars);
            System.Text.StringBuilder stringBuilder = new System.Text.StringBuilder(chineseChars.Length * 4);
            for (int i = 0; i < bytes.Length; i++)
            {
                int num = (int)bytes[i];
                if (num > 160)
                {
                    i++;
                    if (i < bytes.Length)
                    {
                        num = num * 256 + (int)bytes[++i] - 65536;
                    }
                    string text = PinyinHelper.GetPinyin(num);
                    if (!string.IsNullOrEmpty(text))
                    {
                        text = new string(text[0], 1);
                    }
                    stringBuilder.Append(text);
                }
                else if ((num >= 48 && num <= 57) || (num >= 65 && num <= 90) || (num >= 97 && num <= 122))
                {
                    stringBuilder.Append((char)num);
                }
            }
            return stringBuilder.ToString();
        }

        private static string GetPinyin(int charValue)
        {
            string result;
            if (charValue < -20319 || charValue > -10247)
            {
                result = "";
            }
            else
            {
                while (!PinyinHelper._pinyinHash.ContainsKey(charValue))
                {
                    charValue--;
                }
                result = (string)PinyinHelper._pinyinHash[charValue];
            }
            return result;
        }
    }
}
