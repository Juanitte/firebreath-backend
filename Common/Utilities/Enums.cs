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
        AFGHANISTAN, ALBANIA, ALGERIA, ANDORRA, ANGOLA, ANTIGUA_AND_BARBUDA, ARGENTINA, ARMENIA, AUSTRALIA, AUSTRIA,
        AZERBAIJAN, BAHAMAS, BAHRAIN, BANGLADESH, BARBADOS, BELARUS, BELGIUM, BELIZE, BENIN, BHUTAN, BOLIVIA,
        BOSNIA_AND_HERZEGOVINA, BOTSWANA, BRAZIL, BRUNEI, BULGARIA, BURKINA_FASO, BURUNDI, COTE_D_IVOIRE, CABO_VERDE,
        CAMBODIA, CAMEROON, CANADA, CENTRAL_AFRICAN_REPUBLIC, CHAD, CHILE, CHINA, COLOMBIA, COMOROS, CONGO, COSTA_RICA,
        CROATIA, CUBA, CYPRUS, CZECH_REPUBLIC, DEMOCRATIC_REPUBLIC_OF_THE_CONGO, DENMARK, DJIBOUTI, DOMINICA,
        DOMINICAN_REPUBLIC, ECUADOR, EGYPT, EL_SALVADOR, EQUATORIAL_GUINEA, ERITREA, ESTONIA, ESWATINI, ETHIOPIA,
        FIJI, FINLAND, FRANCE, GABON, GAMBIA, GEORGIA, GERMANY, GHANA, GREECE, GRENADA, GUATEMALA, GUINEA, GUINEA_BISSAU,
        GUYANA, HAITI, HOLY_SEE, HONDURAS, HUNGARY, ICELAND, INDIA, INDONESIA, IRAN, IRAQ, IRELAND, ISRAEL, ITALY,
        JAMAICA, JAPAN, JORDAN, KAZAKHSTAN, KENYA, KIRIBATI, KUWAIT, KYRGYZSTAN, LAOS, LATVIA, LEBANON, LESOTHO, LIBERIA,
        LIBYA, LIECHTENSTEIN, LITHUANIA, LUXEMBOURG, MADAGASCAR, MALAWI, MALAYSIA, MALDIVES, MALI, MALTA, MARSHALL_ISLANDS,
        MAURITANIA, MAURITIUS, MEXICO, MICRONESIA, MOLDOVA, MONACO, MONGOLIA, MONTENEGRO, MOROCCO, MOZAMBIQUE, MYANMAR,
        NAMIBIA, NAURU, NEPAL, NETHERLANDS, NEW_ZEALAND, NICARAGUA, NIGER, NIGERIA, NORTH_KOREA, NORTH_MACEDONIA,
        NORWAY, OMAN, PAKISTAN, PALAU, PALESTINE_STATE, PANAMA, PAPUA_NEW_GUINEA, PARAGUAY, PERU, PHILIPPINES, POLAND,
        PORTUGAL, QATAR, ROMANIA, RUSSIA, RWANDA, SAINT_KITTS_AND_NEVIS, SAINT_LUCIA, SAINT_VINCENT_AND_THE_GRENADINES,
        SAMOA, SAN_MARINO, SAO_TOME_AND_PRINCIPE, SAUDI_ARABIA, SENEGAL, SERBIA, SEYCHELLES, SIERRA_LEONE, SINGAPORE,
        SLOVAKIA, SLOVENIA, SOLOMON_ISLANDS, SOMALIA, SOUTH_AFRICA, SOUTH_KOREA, SOUTH_SUDAN, SPAIN, SRI_LANKA, SUDAN,
        SURINAME, SWEDEN, SWITZERLAND, SYRIA, TAJIKISTAN, TANZANIA, THAILAND, TIMOR_LESTE, TOGO, TONGA,
        TRINIDAD_AND_TOBAGO, TUNISIA, TURKEY, TURKMENISTAN, TUVALU, UGANDA, UKRAINE, UNITED_ARAB_EMIRATES, UNITED_KINGDOM,
        UNITED_STATES_OF_AMERICA, URUGUAY, UZBEKISTAN, VANUATU, VENEZUELA, VIETNAM, YEMEN, ZAMBIA, ZIMBABWE, UNDEFINED
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
