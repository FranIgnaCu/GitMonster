using System;
using Godot;

namespace BattleMonster.Scripts;

public partial class MaterialSwapper : Node3D
{
	[Export]
	public Godot.Collections.Array<StandardMaterial3D> Materials = new Godot.Collections.Array<StandardMaterial3D>();
	
	private int _currentMaterial;

	public override void _Ready()
	{
		base._Ready();
		GD.Print("Materials array has: " + Materials[0].AlbedoColor + " and " + Materials[1].AlbedoColor);
	}

	public void NextMaterial()
	{
		
		_currentMaterial += 1;

		if(!(_currentMaterial < Materials.Count)){
			_currentMaterial = 0;
		}

		SwapMaterial(this);
	}

	private void SwapMaterial(Node nodeLookForChildren)
	{
		foreach (var node in nodeLookForChildren.GetChildren())
		{
			var meshInstance = (MeshInstance3D)node;
			try{
				if(meshInstance.IsInGroup("MeshToSwap")){
					meshInstance.SetSurfaceOverrideMaterial(0,Materials[_currentMaterial]);
				}

				SwapMaterial(meshInstance);

			}catch(IndexOutOfRangeException ex){
				if(Materials.Count == 0){
					GD.Print("Empty Materials List In MaterialSwapper");

				}else{
					GD.Print("Invalid Index: " + _currentMaterial + " for swaping materials; error message: " + ex.Message);
				}
			}catch(Exception ex){
				GD.Print("Error Inesperado en Material Swapper: " + ex.Message);
			}
		}
	}

	public void SwapMaterialForIndex(int indexOfDesiredMaterial){

		if(indexOfDesiredMaterial < Materials.Count && 0<=indexOfDesiredMaterial){
			_currentMaterial = indexOfDesiredMaterial;

			SwapMaterial(this);
		}else{
			GD.Print("Invalid index" + indexOfDesiredMaterial);
		}
	}
}