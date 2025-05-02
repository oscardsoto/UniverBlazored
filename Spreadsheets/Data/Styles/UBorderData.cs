
namespace UniverBlazored.Spreadsheets.Data.Styles
{
    /// <summary>
    /// IBorderData, from Univer (See doc: https://univer.ai/typedoc/@univerjs/core/interfaces/IBorderData)
    /// </summary>
    public struct UBorderData
    {
        /// <summary>
        /// Bottom
        /// </summary>
        public UBorderStyleData? b { get; set; }

        /// <summary>
        /// START_BOTTOM_CENTER_END_TOP_RIGHT
        /// </summary>
        public UBorderStyleData? bc_tr { get; set; }

        /// <summary>
        /// START_BOTTOM_LEFT_END_TOP_RIGHT
        /// </summary>
        public UBorderStyleData? bl_tr { get; set; }

        /// <summary>
        /// Left
        /// </summary>
        public UBorderStyleData? l { get; set; }

        /// <summary>
        /// START_MIDDLE_LEFT_END_TOP_RIGHT
        /// </summary>
        public UBorderStyleData? ml_tr { get; set; }

        /// <summary>
        /// Right
        /// </summary>
        public UBorderStyleData? r { get; set; }

        /// <summary>
        /// Top
        /// </summary>
        public UBorderStyleData? t { get; set; }

        /// <summary>
        /// START_TOP_LEFT_END_BOTTOM_CENTER
        /// </summary>
        public UBorderStyleData? tl_bc { get; set; }

        /// <summary>
        /// START_TOP_LEFT_END_BOTTOM_RIGHT
        /// </summary>
        public UBorderStyleData? tl_br { get; set; }

        /// <summary>
        /// START_MIDDLE_LEFT_END_TOP_RIGHT
        /// </summary>
        public UBorderStyleData? tl_mr { get; set; }
    }

}