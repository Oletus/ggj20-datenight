import bpy
import os
import webbrowser

fold_dst = ""
dice_name = ""
test_value = 0.2

centerExp = True

ArmatureName = 'Avatar_Rig'

def Activate_Rig():
	global ArmatureName
	ArmatureName = bpy.context.object.name


#http://www.blender.org/documentation/blender_python_api_2_59_0/bpy.path.html
  
from bpy.props import (StringProperty,
                       BoolProperty,
                       IntProperty,
                       FloatProperty,
                       FloatVectorProperty,
                       EnumProperty,
                       PointerProperty,
                       )

from bpy.types import (Panel,
                       Operator,
                       AddonPreferences,
                       PropertyGroup,
                       )


# ------------------------------------------------------------------------
#    store properties in the active scene
# ------------------------------------------------------------------------

class MySettings(PropertyGroup):

    my_bool = BoolProperty(
        name="Enable or Disable",
        description="A bool property",
        default = False
        )


##################
## Rotation Fix ##
##################
def FixRotate(name):
    # Rotating mesh if needed
    if bpy.data.objects[name].rotation_euler.x == 0.0:
        bpy.data.objects[name].rotation_euler.x = 1.5707964897155762
        if bpy.data.objects[name].type != 'EMPTY':
            bpy.ops.object.mode_set(mode='EDIT')
            bpy.ops.mesh.select_all(action='SELECT')
        else:
            for child in bpy.data.objects[name].children:
                child.select = True
            bpy.ops.object.transform_apply(location=False, rotation=True, scale=False)
        bpy.context.scene.cursor_location = (0.0, 0.0, 0.0)
        bpy.context.space_data.pivot_point = 'CURSOR'
        bpy.ops.transform.rotate(value = 1.5707964897155762, axis=(-1, 0, 0), constraint_orientation='GLOBAL')
        bpy.ops.object.mode_set(mode='OBJECT')
        if bpy.data.objects[name].type == 'EMPTY':
            bpy.ops.object.transform_apply(location=False, rotation=True, scale=False)


###################
##  Open Manual  ##
###################
def ffOpenManual():
    webbrowser.open('https://docs.google.com/document/d/1P060VEOZgjDDxcoZD2OQ3ZyXb8l6GuG3V6_kqi1OK_A/edit?ts=5cfe1295#heading=h.j33psldl5hrd')


#######################
# Tile Export Rigged #
#######################
# Smart export rigged read object name an base on it it will save file to right folder.
# 
def ffTileExport():
    #bpy.ops.paint.vertex_paint_toggle()
    #bpy.ops.paint.vertex_paint_toggle()
    #print(fold_dst + " 2")    
    name = bpy.context.object.name
    
    #Add vertex color if needed #
    bpy.ops.object.select_all(action='DESELECT')
    #bpy.context.scene.objects.active = bpy.data.objects[name]
    ##FixRotate(name)
    children = bpy.context.object.children
    for i in children:
        print (i.name)
        bpy.data.objects[name].select_set(True)
        #bpy.context.scene.objects.active = bpy.data.objects[i.name]
        #bpy.ops.paint.vertex_paint_toggle()
        #bpy.ops.paint.vertex_paint_toggle()
        #FixRotate(i.name)
        bpy.ops.object.select_all(action='DESELECT')
    
    # Exporting #
    

    bpy.ops.object.select_all(action='DESELECT')
    #bpy.context.scene.objects.active = bpy.data.objects[name]
    bpy.data.objects[name].select_set(True)
    for child in bpy.data.objects[name].children:
        child.select_set(True)
    
    
      
    print(fold_dst +name)
    #print(fold_dst[-7:-1])
    dst_name = ""

    dst_name = fold_dst+name             
    
    print(centerExp)
        
    # Rotating mesh if needed
    #FixRotate(name)
    
    #bpy.data.objects['Avatar_Rig'].select = True
    bpy.ops.export_scene.fbx(filepath=dst_name+".fbx", check_existing=True, 
        axis_forward='Z', axis_up='Y', 
        filter_glob="*.fbx", #version='BIN7400',  
        use_selection=True, 
        global_scale=1, 
        apply_unit_scale=False, 
        apply_scale_options='FBX_SCALE_ALL', 
        bake_space_transform=True, 
        object_types={'EMPTY', 'ARMATURE', 'MESH', 'OTHER'}, 
        use_mesh_modifiers=True, use_mesh_modifiers_render=True, 
        mesh_smooth_type='OFF', use_mesh_edges=False, use_tspace=False, 
        use_custom_props=False, 
        add_leaf_bones=False, 
        primary_bone_axis='Y', secondary_bone_axis='X', 
        use_armature_deform_only=True, 
        armature_nodetype='NULL', 
        bake_anim=True, 
        bake_anim_use_all_bones=True, 
        bake_anim_use_nla_strips=False, 
        bake_anim_use_all_actions=False, 
        bake_anim_force_startend_keying=True, 
        bake_anim_step=1, 
        bake_anim_simplify_factor=1, 
        #use_anim=True, 
        #use_anim_action_all=True, 
        #use_default_take=True, 
        #use_anim_optimize=True, 
        #anim_optimize_precision=6, 
        path_mode='AUTO', 
        embed_textures=False, 
        batch_mode='OFF', 
        use_batch_own_dir=True, 
        use_metadata=True)
        
    print(dst_name+".fbx")



#######################
# Smart Export Rigged #
#######################
# Smart export rigged read object name an base on it it will save file to right folder.
# 
def ffSmartExport():
    bpy.ops.paint.vertex_paint_toggle()
    bpy.ops.paint.vertex_paint_toggle()
    #print(fold_dst + " 2")    
    name = bpy.context.object.name  
    print(fold_dst +name)
    #print(fold_dst[-7:-1])
    dst_name = ""

    dst_name = fold_dst+name             
    
    print(centerExp)
        
    # Rotating mesh if needed
    #FixRotate(name)
    
    #bpy.data.objects['Avatar_Rig'].select = True
    bpy.ops.export_scene.fbx(filepath=dst_name+".fbx", check_existing=True, 
        axis_forward='Z', axis_up='Y', 
        filter_glob="*.fbx", #version='BIN7400', 
        ui_tab='MAIN', 
        use_selection=True, 
        global_scale=1, 
        apply_unit_scale=False, 
        apply_scale_options='FBX_SCALE_ALL', 
        bake_space_transform=True, 
        object_types={'EMPTY', 'ARMATURE', 'MESH', 'OTHER'}, 
        use_mesh_modifiers=True, use_mesh_modifiers_render=True, 
        mesh_smooth_type='OFF', use_mesh_edges=False, use_tspace=False, 
        use_custom_props=False, 
        add_leaf_bones=False, 
        primary_bone_axis='Y', secondary_bone_axis='X', 
        use_armature_deform_only=True, 
        armature_nodetype='NULL', 
        bake_anim=True, 
        bake_anim_use_all_bones=True, 
        bake_anim_use_nla_strips=False, 
        bake_anim_use_all_actions=False, 
        bake_anim_force_startend_keying=True, 
        bake_anim_step=1, 
        bake_anim_simplify_factor=1, 
        #use_anim=True, 
        #use_anim_action_all=True, 
        #use_default_take=True, 
        #use_anim_optimize=True, 
        #anim_optimize_precision=6, 
        path_mode='AUTO', 
        embed_textures=False, 
        batch_mode='OFF', 
        use_batch_own_dir=True, 
        use_metadata=True)
        
    print(dst_name+".fbx")



#
#    Menu in UI region
#

class View3DPanel:
    bl_space_type = 'VIEW_3D'
    bl_region_type = 'UI'
    bl_category = "Date Night"
    bl_context = "objectmode"

    @classmethod
    def poll(cls, context):
        return (context.object is not None)

class LockpicklePanel(View3DPanel, bpy.types.Panel):
    #"""A Lockpickle Panel in the Viewport Toolbar"""
    bl_idname = "VIEW3D_PT_Lockpickle_Exporter"
    bl_label = "Lockpickle Panel"

    
    def draw(self, context):
        layout = self.layout
        
        obj = context.object
        global fold_dst
        global dice_name
        fold_dst = context.scene.destination
        
        row = layout.row(align=False)
        row.label(text="Export to Unity:")
        row.alignment = 'RIGHT'
        row.operator("open.manual", text="",icon='QUESTION')
        row = layout.row()
        row.prop(bpy.context.scene,"destination")
        
        fold_dst = context.scene.destination
        fold_dst = bpy.path.abspath(fold_dst)
        fold_dst = fold_dst.replace("\\","/")

        #fold_dst = os.listdir(fold_dst)
        
        split = layout.split(align=True)
        col = split.column(align=True)
        scene = context.scene
        #mytool = scene.my_tool
        #col.prop(mytool, "my_bool", text="Center")
        
        # check if bool property is enabled
        #if (mytool.my_bool == True):
        #    centerExp = True
        #else:
        #    centerExp = False
        
        col.operator("tile.export", text="Multi Object Export", icon='MESH_PLANE')
        #col.operator("smart.export", text="Export", icon='MESH_ICOSPHERE')
        
     
        row = layout.row()
        #centerExp = bpy.props.BoolProperty(name= 'Test')
        #row.prop(centerExp, "boolean")
        
        split = layout.split(align=True)
        col = split.column(align=True)
        


        
# Teen operaatorin jotta voin kutsua sitä napissa.



class SmartExport(bpy.types.Operator):
    "Export to wanted folder"
    bl_idname = "smart.export"
    bl_label = "Smart Export"
 
    def execute(self, context):
        ffSmartExport()
        return{'FINISHED'}

class TileExport(bpy.types.Operator):
    "Export Tile to wanted folder"
    bl_idname = "tile.export"
    bl_label = "Tile Export"
 
    def execute(self, context):
        ffTileExport()
        return{'FINISHED'}  
    


class OpenManual(bpy.types.Operator):
    bl_idname = "open.manual"
    bl_label = "Open Manual"
 
    def execute(self, context):
        ffOpenManual()
        return{'FINISHED'}   

    
#bpy.utils.register_module(__name__)

bpy.types.Scene.destination = bpy.props.StringProperty(  name="Destination", description = "Directory file to go", subtype='DIR_PATH' )
bpy.types.Scene.fbxFile = bpy.props.StringProperty(  name="FBX File", description = "File You want to Fix", subtype='FILE_PATH' )


bpy.utils.register_class(SmartExport)
bpy.utils.register_class(TileExport)
bpy.utils.register_class(OpenManual)
bpy.utils.register_class(LockpicklePanel)

#bpy.utils.register_module(__name__)

# ------------------------------------------------------------------------
# register and unregister functions
# ------------------------------------------------------------------------




#def register():
#    bpy.utils.register_module(__name__)
#    bpy.types.Scene.my_tool = PointerProperty(type=MySettings)

#def unregister():
#    bpy.utils.unregister_module(__name__)
#    del bpy.types.Scene.my_tool



              

