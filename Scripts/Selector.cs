using Godot;

namespace BattleMonster.Scripts;

// En el juego debo recordar de cambiar de Node3D a Mesh e intentar agrupar el selector en uno
// Debera de crear un grupo donde meter todas las mesh que quiero que cambien
// Esto para poder cambiar el material entre selected y unselected y poder hacerlo con 
// distintos Mesh si en algún momento quiero cambiar el diseño y asi mantener algunos sin cambio
public interface ISelectable{
    void InformSelected();
    void InformUnSelected();

}

public partial class Selector : Node3D
{
    ISelectable _owner;
    private bool _selected;
    

    public override void _Ready()
    {
        Hide();
        _owner = null;
    }

    public override void _PhysicsProcess(double delta)
    {
        if (_owner != null) return;
        
        GD.PrintErr("Falta Definir Owner en el selector");
        GetTree().Quit();
    }

    public void ForObject(ISelectable ownerObject){
        if (_owner == null || _owner == ownerObject)
        {
            _owner = ownerObject;
        }
        else
        {
            GD.PrintErr("Dos Objetos Intentaron Usar el Mismo Selector");
            GetTree().Quit();
        }
    }

    public bool IsSelected(){
        return _selected;
    }

    public void Select(){
        if(!_selected){
            SwapToMaterial(1);
        }
        _selected = true;
        Show();

        _owner.InformSelected();
    }
    public void UnSelect(){
        if(_selected)
        {
            SwapToMaterial(0);
        }
        _selected = false;
        _owner.InformUnSelected();
    }


    private void SwapToMaterial(int indexOfMaterialToSwap)
    {
        var swapper = GetNode<MaterialSwapper>("material_swapper");
        
        swapper.SwapMaterialForIndex(indexOfMaterialToSwap);
        
    }


    public void Clicked(){
        if(_selected){
            UnSelect();
        }else{
            Select();
        }
    }

    public void MouseEnteredObject()
    {
        Show();
    }
    public void MouseExitedObject()
    {
        if (!IsSelected())
        {
            Hide();
        }
    }
}