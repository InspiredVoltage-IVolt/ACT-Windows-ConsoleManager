using Newtonsoft.Json;
using System.Collections.Generic;

namespace ACT.Applications.ConsoleManager.Structs
{

    public class ACT_Console_MenuConfig
    {
        [JsonProperty("id", NullValueHandling = NullValueHandling.Ignore)]
        public string Id { get; set; }

        [JsonProperty("display_name", NullValueHandling = NullValueHandling.Ignore)]
        public string DisplayName { get; set; }

        [JsonProperty("menu_header_markupfile", NullValueHandling = NullValueHandling.Ignore)]
        public string MenuHeaderMarkupfile { get; set; }

        [JsonProperty("menu_footer_markupfile", NullValueHandling = NullValueHandling.Ignore)]
        public string MenuFooterMarkupfile { get; set; }

        [JsonProperty("default_background", NullValueHandling = NullValueHandling.Ignore)]
        public string DefaultBackground { get; set; }

        [JsonProperty("default_foreground", NullValueHandling = NullValueHandling.Ignore)]
        public string DefaultForeground { get; set; }

        [JsonProperty("default_width", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Helper.ParseStringConverter))]
        public long? DefaultWidth { get; set; }

        [JsonProperty("default_height", NullValueHandling = NullValueHandling.Ignore)]
        [JsonConverter(typeof(Helper.ParseStringConverter))]
        public long? DefaultHeight { get; set; }

        [JsonProperty("require_enter_button_press", NullValueHandling = NullValueHandling.Ignore)]
        public bool? RequireEnterButtonPress { get; set; }

        [JsonProperty("login_menu_keycode", NullValueHandling = NullValueHandling.Ignore)]
        public string LoginMenuKeycode { get; set; }

        [JsonProperty("login_menu_item_visible", NullValueHandling = NullValueHandling.Ignore)]
        public bool? LoginMenuItemVisible { get; set; }

        [JsonProperty("login_menu_position", NullValueHandling = NullValueHandling.Ignore)]
        public string LoginMenuPosition { get; set; }

        [JsonProperty("login_menu_show_on_all_menus", NullValueHandling = NullValueHandling.Ignore)]
        public bool? LoginMenuShowOnAllMenus { get; set; }

        [JsonProperty("login_menu_caption", NullValueHandling = NullValueHandling.Ignore)]
        public string LoginMenuCaption { get; set; }

        [JsonProperty("login_menu_display_markupfile", NullValueHandling = NullValueHandling.Ignore)]
        public string LoginMenuDisplayMarkupfile { get; set; }

        [JsonProperty("login_menu_method_name", NullValueHandling = NullValueHandling.Ignore)]
        public string LoginMenuMethodName { get; set; }

        [JsonProperty("admin_mode_keycode", NullValueHandling = NullValueHandling.Ignore)]
        public string AdminModeKeycode { get; set; }

        [JsonProperty("admin_mode_menu_item_visible", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AdminModeMenuItemVisible { get; set; }

        [JsonProperty("admin_mode_id", NullValueHandling = NullValueHandling.Ignore)]
        public long? AdminModeId { get; set; }

        [JsonProperty("admin_mode_caption", NullValueHandling = NullValueHandling.Ignore)]
        public string AdminModeCaption { get; set; }

        [JsonProperty("admin_mode_show_on_all_menus", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AdminModeShowOnAllMenus { get; set; }

        [JsonProperty("admin_mode_position", NullValueHandling = NullValueHandling.Ignore)]
        public string AdminModePosition { get; set; }

        [JsonProperty("admin_mode_always_show_admin_header", NullValueHandling = NullValueHandling.Ignore)]
        public bool? AdminModeAlwaysShowAdminHeader { get; set; }

        [JsonProperty("admin_header_display_markupfile", NullValueHandling = NullValueHandling.Ignore)]
        public string AdminHeaderDisplayMarkupfile { get; set; }

        [JsonProperty("admin_menu_option_markupfile", NullValueHandling = NullValueHandling.Ignore)]
        public string AdminMenuOptionMarkupfile { get; set; }

        [JsonProperty("permissions_file", NullValueHandling = NullValueHandling.Ignore)]
        public string PermissionsFile { get; set; }

        [JsonProperty("admin_menu_items", NullValueHandling = NullValueHandling.Ignore)]
        public List<Menu_Item> AdminMenuItems { get; set; }

        public static ACT_Console_MenuConfig FromJson(string json) => JsonConvert.DeserializeObject<ACT_Console_MenuConfig>(json, Helper.Converter.Settings);

        public string ToJson() => JsonConvert.SerializeObject(this, Helper.Converter.Settings);
    }


}
