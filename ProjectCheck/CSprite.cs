using System;
using System.Collections.Generic;
using System.Drawing;
using System.Drawing.Imaging;
using System.IO;
using System.Linq;
using System.Runtime.InteropServices;
using System.Text;
using System.Threading.Tasks;

namespace ProjectCheck
{
    class CSprite
    {
        private string path = null;

        private SubentFileHead file_head = new SubentFileHead ();

        public CSprite(string path)
        {
            this.path = path;
            Init();
        }

        private void Init()
        {
            FileInfo file = new FileInfo(this.path);
            using (FileStream fs = file.OpenRead())
            {
                using (BinaryReader br = new BinaryReader(fs))
                {
                    byte[] file_head_bytes = br.ReadBytes(StructOperateHelper.GetStructSize(typeof(SubentFileHead)));
                    file_head = (SubentFileHead)StructOperateHelper.BytesToStruct(file_head_bytes, typeof(SubentFileHead));
                    Console.ReadLine();
                }
            }
        }

        private void ExportImages()
        {
            FileInfo file = new FileInfo(this.path);
            using (FileStream fs = file.OpenRead())
            {
                using (BinaryReader br = new BinaryReader(fs))
                {

                    br.BaseStream.Seek(file_head.embeded_sprite_res_offset, SeekOrigin.Begin);
                    byte[] embed_sprite_res = new byte[file_head.embeded_sprite_res_datalen];

                    br.BaseStream.Seek(file_head.embeded_frame_res_offset, SeekOrigin.Begin);
                    byte[] embed_frame_res = new byte[file_head.embeded_frame_res_datalen];

                    br.BaseStream.Seek(file_head.embeded_ref_sprite_res_offset, SeekOrigin.Begin);
                    byte[] embed_ref_sprite_res = new byte[file_head.embeded_ref_sprite_res_datalen];

                    br.BaseStream.Seek(file_head.ref_sprite_res_offset, SeekOrigin.Begin);
                    byte[] ref_sprite_res = new byte[file_head.ref_sprite_res_datalen];

                    br.BaseStream.Seek(file_head.embed_img_info_offset, SeekOrigin.Begin);
                    byte[] embed_img_info = new byte[file_head.embed_img_info_datalen];

                    br.BaseStream.Seek(file_head.embed_img_data_offset, SeekOrigin.Begin);
                    byte[] embed_img_data = new byte[file_head.embed_img_data_datalen];
                }
            }
        }
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct SubentFileHead
    {
        public UInt32 fourcc;		                // 文件签名信息,始终为SUBENT_FILE_FOUCC
        public UInt32 version;		                // 文件版本信息,始终为SUBENT_FILE_VERSION
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 56)]
        public string copyright;	                // 文件版本信息
        public UInt32 res_scale;                    // 资源缩放比
        public UInt32 compress_ratio;               // 图片压缩比

        public byte use_mode;                       // 使用模式(互斥模式,共生模式)

        public UInt16 total_embed_sprite;
        public UInt16 total_embed_ref_sprite;       // 引用内部精灵总数
        public UInt16 total_ref_sprite;	            // 总精灵数量
        public UInt16 total_embed_image;			// 总纹理数量

        public UInt32 embeded_sprite_res_offset;    // 内嵌精灵资源偏址
        public UInt32 embeded_sprite_res_datalen;   // 内嵌精灵资源长度

        public UInt32 embeded_frame_res_offset;	    // 内嵌帧资源偏址
        public UInt32 embeded_frame_res_datalen;	// 内嵌帧资源长度

        public UInt32 embeded_ref_sprite_res_offset;    // 引用精灵资源偏址
        public UInt32 embeded_ref_sprite_res_datalen;   // 引用精灵资源长度

        public UInt32 ref_sprite_res_offset;		// 引用精灵资源偏址
        public UInt32 ref_sprite_res_datalen;		// 引用精灵资源长度

        public UInt32 embed_img_info_offset;		// 图片信息偏移位置
        public UInt32 embed_img_info_datalen;		// 图片信息总长

        public UInt32 embed_img_data_offset;		// 纹理数据信息的偏移
        public UInt32 embed_img_data_datalen;	    // 纹理数据信息总长

        public UInt32 segment_total_count;
        public UInt32 segment_img_data_offset;
        public UInt32 segment_img_data_datalen;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct FileRefSpriteInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string ref_anim_name;				// 引用的精灵动作名称

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string ref_csprite_name;             // 引用的csprite名称
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct FileEmbedRefSpriteInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string ref_alias_name;				// 引用的精灵动作别名

        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string ref_amin_name;				// 引用的精灵动作名称
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    struct FileEmbedSpriteInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string anim_name;	                // 动作名称(如"stat0")
        public byte is_contain_reverse;	            // 是否包含反向精灵
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string reverse_anim_name;	        // 反向动作名称	

        public byte draw_mode;
        public byte loop_mode;
        public UInt32 play_frequency;

        public Int16 aabb_left;
        public Int16 aabb_top;
        public Int16 aabb_right;
        public Int16 aabb_bottom;

        public Int16 inv_aabb_left;
        public Int16 inv_aabb_top;
        public Int16 inv_aabb_right;
        public Int16 inv_aabb_bottom;

        public Int16 sort_x;
        public Int16 sort_y;

        public byte total_frame;

        public UInt32 frame_info_offset;            // 帧基本信息的相对存储偏移位置
        public UInt32 frame_info_datalen;           // 帧基本信息总长


    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct FileEmbedFrameInfo
    {
        public float uv_left;
        public float uv_top;
        public float uv_right;
        public float uv_bottom;

        public float offset_x;
        public float offset_y;

        public byte is_rotated;

        public Int16 source_size_width;
        public Int16 source_size_height;

        public Int16 source_color_x;
        public Int16 source_color_y;
        public Int16 source_color_width;
        public Int16 source_color_height;

        public UInt32 blend_color;
        public UInt32 custom_frame_time;

        public UInt16 embed_image_id;		            // 嵌入图片的资源id
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct FileEmbedImageInfo
    {
        //[MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public UInt16 embed_image_id;		            // 嵌入图片的资源id

        public byte img_data_code_type;
        public UInt32 img_data_offset;
        public UInt32 img_data_size;
        public UInt32 img_alpha_offset;
        public UInt32 img_alpha_size;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct FileMyRect
    {
        public UInt32 left;
        public UInt32 top;
        public UInt32 width;
        public UInt32 height;
    }

    [StructLayoutAttribute(LayoutKind.Sequential, CharSet = CharSet.Ansi, Pack = 1)]
    public struct FileSegInfo
    {
        [MarshalAs(UnmanagedType.ByValTStr, SizeConst = 64)]
        public string anim_name;				// 引用的csprite名称

        public UInt32 count;
        public UInt32 type;
        public byte is_center;
    }
}
