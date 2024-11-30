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

    /// <summary>
    ///     Tipos de campo para las tablas
    /// </summary>
    public enum FieldType
    {
        TINYINT = 0,
        SMALLINT = 1,
        INT = 2,
        BIGINT = 3,
        BIT = 4,
        DECIMAL = 5,
        DATE = 6,
        TIME = 7,
        DATETIME = 8,
        CHAR = 9,
        VARCHAR = 10,
        BINARY = 11
    }

    /// <summary>
    ///     Tipos de dato para los atributos
    /// </summary>
    public enum AttributeType
    {
        INT = 0,
        BOOL = 1,
        FLOAT = 2,
        DATE = 3,
        TIME = 4,
        DATETIME = 5,
        CHAR = 6,
        STRING = 7
    }

    /// <summary>
    ///     Tipos de relaciones entre tablas
    /// </summary>
    public enum Relations
    {
        NONE = -1,
        ONE_TO_ONE = 0,
        ONE_TO_MANY = 1,
        MANY_TO_ONE = 2,
        MANY_TO_MANY = 3
    }

    /// <summary>
    ///     Tipos de operadores de comparación
    /// </summary>
    public enum CompareOperator
    {
        LESS = 0,
        LESS_EQUAL = 1,
        EQUAL = 2,
        GREATER = 3,
        GREATER_EQUAL = 4
    }

    /// <summary>
    ///     Tipos de prioridades de incidencia
    /// </summary>
    public enum Priorities
    {
        ALL = -1,
        NOT_SURE = 0,
        LOWEST = 1,
        LOW = 2,
        MEDIUM = 3,
        HIGH = 4,
        HIGHEST = 5
    }

    /// <summary>
    ///     Tipos de estados de incidencia
    /// </summary>
    public enum Status
    {
        ALL = -1,
        PENDING = 0,
        OPENED = 1,
        PAUSED = 2,
        FINISHED = 3
    }

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
