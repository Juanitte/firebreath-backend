using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;
using System.Threading.Tasks;

namespace Common.Utilities
{
    /// <summary>
    ///     Tipos de orden
    /// </summary>
    public enum OrderType
    {
        up,
        down
    }

    /// <summary>
    ///     Tipos de filtrado para las tablas
    /// </summary>
    public enum FilterType
    {
        contains,
        equals,
        greatherThan,
        greatherThanEqual,
        isNullOrEmpty,
        lessThan,
        lessThanEqual,
        notEquals
    }

    public enum AttachmentContainerType
    {
        POST,
        MESSAGE
    }

    /// <summary>
    ///     Países del mundo
    /// </summary>
    public enum Country
    {

        UNDEFINED = 0,
        AFGHANISTAN = 1,
        ALBANIA = 2,
        ALGERIA = 3,
        ANDORRA = 4,
        ANGOLA = 5,
        ANTIGUA_AND_BARBUDA = 6,
        ARGENTINA = 7,
        ARMENIA = 8,
        AUSTRALIA = 9,
        AUSTRIA = 10,
        AZERBAIJAN = 11,
        BAHAMAS = 12,
        BAHRAIN = 13,
        BANGLADESH = 14,
        BARBADOS = 15,
        BELARUS = 16,
        BELGIUM = 17,
        BELIZE = 18,
        BENIN = 19,
        BHUTAN = 20,
        BOLIVIA = 21,
        BOSNIA_AND_HERZEGOVINA = 22,
        BOTSWANA = 23,
        BRAZIL = 24,
        BRUNEI = 25,
        BULGARIA = 26,
        BURKINA_FASO = 27,
        BURUNDI = 28,
        COTE_D_IVOIRE = 29,
        CABO_VERDE = 30,
        CAMBODIA = 31,
        CAMEROON = 32,
        CANADA = 33,
        CENTRAL_AFRICAN_REPUBLIC = 34,
        CHAD = 35,
        CHILE = 36,
        CHINA = 37,
        COLOMBIA = 38,
        COMOROS = 39,
        CONGO = 40,
        COSTA_RICA = 41,
        CROATIA = 42,
        CUBA = 43,
        CYPRUS = 44,
        CZECH_REPUBLIC = 45,
        DEMOCRATIC_REPUBLIC_OF_THE_CONGO = 46,
        DENMARK = 47,
        DJIBOUTI = 48,
        DOMINICA = 49,
        DOMINICAN_REPUBLIC = 50,
        ECUADOR = 51,
        EGYPT = 52,
        EL_SALVADOR = 53,
        EQUATORIAL_GUINEA = 54,
        ERITREA = 55,
        ESTONIA = 56,
        ESWATINI = 57,
        ETHIOPIA = 58,
        FIJI = 59,
        FINLAND = 60,
        FRANCE = 61,
        GABON = 62,
        GAMBIA = 63,
        GEORGIA = 64,
        GERMANY = 65,
        GHANA = 66,
        GREECE = 67,
        GRENADA = 68,
        GUATEMALA = 69,
        GUINEA = 70,
        GUINEA_BISSAU = 71,
        GUYANA = 72,
        HAITI = 73,
        HOLY_SEE = 74,
        HONDURAS = 75,
        HUNGARY = 76,
        ICELAND = 77,
        INDIA = 78,
        INDONESIA = 79,
        IRAN = 80,
        IRAQ = 81,
        IRELAND = 82,
        ISRAEL = 83,
        ITALY = 84,
        JAMAICA = 85,
        JAPAN = 86,
        JORDAN = 87,
        KAZAKHSTAN = 88,
        KENYA = 89,
        KIRIBATI = 90,
        KUWAIT = 91,
        KYRGYZSTAN = 92,
        LAOS = 93,
        LATVIA = 94,
        LEBANON = 95,
        LESOTHO = 96,
        LIBERIA = 97,
        LIBYA = 98,
        LIECHTENSTEIN = 99,
        LITHUANIA = 100,
        LUXEMBOURG = 101,
        MADAGASCAR = 102,
        MALAWI = 103,
        MALAYSIA = 104,
        MALDIVES = 105,
        MALI = 106,
        MALTA = 107,
        MARSHALL_ISLANDS = 108,
        MAURITANIA = 109,
        MAURITIUS = 110,
        MEXICO = 111,
        MICRONESIA = 112,
        MOLDOVA = 113,
        MONACO = 114,
        MONGOLIA = 115,
        MONTENEGRO = 116,
        MOROCCO = 117,
        MOZAMBIQUE = 118,
        MYANMAR = 119,
        NAMIBIA = 120,
        NAURU = 121,
        NEPAL = 122,
        NETHERLANDS = 123,
        NEW_ZEALAND = 124,
        NICARAGUA = 125,
        NIGER = 126,
        NIGERIA = 127,
        NORTH_KOREA = 128,
        NORTH_MACEDONIA = 129,
        NORWAY = 130,
        OMAN = 131,
        PAKISTAN = 132,
        PALAU = 133,
        PALESTINE_STATE = 134,
        PANAMA = 135,
        PAPUA_NEW_GUINEA = 136,
        PARAGUAY = 137,
        PERU = 138,
        PHILIPPINES = 139,
        POLAND = 140,
        PORTUGAL = 141,
        QATAR = 142,
        ROMANIA = 143,
        RUSSIA = 144,
        RWANDA = 145,
        SAINT_KITTS_AND_NEVIS = 146,
        SAINT_LUCIA = 147,
        SAINT_VINCENT_AND_THE_GRENADINES = 148,
        SAMOA = 149,
        SAN_MARINO = 150,
        SAO_TOME_AND_PRINCIPE = 151,
        SAUDI_ARABIA = 152,
        SENEGAL = 153,
        SERBIA = 154,
        SEYCHELLES = 155,
        SIERRA_LEONE = 156,
        SINGAPORE = 157,
        SLOVAKIA = 158,
        SLOVENIA = 159,
        SOLOMON_ISLANDS = 160,
        SOMALIA = 161,
        SOUTH_AFRICA = 162,
        SOUTH_KOREA = 163,
        SOUTH_SUDAN = 164,
        SPAIN = 165,
        SRI_LANKA = 166,
        SUDAN = 167,
        SURINAME = 168,
        SWEDEN = 169,
        SWITZERLAND = 170,
        SYRIA = 171,
        TAJIKISTAN = 172,
        TANZANIA = 173,
        THAILAND = 174,
        TIMOR_LESTE = 175,
        TOGO = 176,
        TONGA = 177,
        TRINIDAD_AND_TOBAGO = 178,
        TUNISIA = 179,
        TURKEY = 180,
        TURKMENISTAN = 181,
        TUVALU = 182,
        UGANDA = 183,
        UKRAINE = 184,
        UNITED_ARAB_EMIRATES = 185,
        UNITED_KINGDOM = 186,
        UNITED_STATES_OF_AMERICA = 187,
        URUGUAY = 188,
        UZBEKISTAN = 189,
        VANUATU = 190,
        VENEZUELA = 191,
        VIETNAM = 192,
        YEMEN = 193,
        ZAMBIA = 194,
        ZIMBABWE = 195
    }

    /// <summary>
    ///     Idiomas
    /// </summary>
    public enum Language
    {
        English = 1,
        Spanish = 2
    }

    /// <summary>
    ///		Especificación de los códigos de error
    /// </summary>
    public enum ResponseCodes
    {
        /// <summary>
        ///		Valor de Ok
        /// </summary>
        Ok = 0,
        /// <summary>
        ///	Valor de ErrorToken 
        /// </summary>
        ErrorToken = 1,
        /// <summary>
        ///		Valor de OriginAccess 
        /// </summary>
		OriginAccess = 2,
        /// <summary>
        ///		Valor de InvalidUserName
        /// </summary>
		InvalidUserName = 3,
        /// <summary>
        ///		Valor de InvalidPassword
        /// </summary>
        InvalidPassword = 4,
        /// <summary>
        ///		Valor de BdConectionFailed
        /// </summary>
        BdConectionFailed = 5,
        /// <summary>
        ///		Valor de NoDataFound
        /// </summary>
        NoDataFound = 6,
        /// <summary>
        ///		Valor de SaveDataFailed
        /// </summary>
        SaveDataFailed = 7,
        /// <summary>
        ///		Valor de SinEmpresa
        /// </summary>
        SinEmpresa = 8,
        /// <summary>
        ///		Valor de AccesoNoAutorizado
        /// </summary>
        AccesoNoAutorizado = 9,
        /// <summary>
        ///		Valor de ErrorDatos
        /// </summary>
        DataError = 10,
        /// <summary>
        ///		Valor de OtherError
        /// </summary>
        OtherError = 11,
        /// <summary>
        ///		Valor de InvalidModel
        /// </summary>
        InvalidModel = 12,
        /// <summary>
        ///		Valor de InvalidAccessType
        /// </summary>
        InvalidAccessType = 13,
        /// <summary>
        ///		Valor de InvalidToken
        /// </summary>
        InvalidToken = 14
    }
}
