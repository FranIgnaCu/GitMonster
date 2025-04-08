extends GutTest


var MyScene = preload('res://Tests/Scenes/movement_test.tscn')
var scene = null
var player = null

func before_each():
	scene = MyScene.instantiate()
	get_tree().root.add_child(scene)
	player = scene.get_node('playerControl')

func after_each():
	# Liberar los nodos hijos y la escena después de cada test
	if scene:
		scene.queue_free()
	player = null

func assert_PositionCentersTo(clickedPosition, centeredPosition):
	var actualPositin = player.centerInTile(clickedPosition)
	assert_eq(actualPositin, centeredPosition)
	
func assert_onTile(actualPosition,destinyTile):

	assert_almost_eq(actualPosition.distance_to(destinyTile), 0.0, 0.15)
	

func test_evenNotPairsGetIncreasedInOne():
	assert_PositionCentersTo(Vector3(3,3,7), Vector3(4,3,8))

func test_HightIsNotTranslated():
	assert_PositionCentersTo(Vector3(2,4,6), Vector3(2,4,6))

func test_evenNumbersGetFlattend():
	assert_PositionCentersTo(Vector3(2.2,3,8.6), Vector3(2,3,8))

func test_unEvenNumbersBecomeNextEvenNumber():
	assert_PositionCentersTo(Vector3(1.2,3,7.5), Vector3(2,3,8))

func test_negativeNumbersdontBecomePositive():
	assert_PositionCentersTo(Vector3(-1.2,3,-7.5), Vector3(-2,3,-8))

func test_moveSelectedCharacterToATileMovesHim():
	var troop = scene.get_node("Squad/BaseTroop")
	troop.clicked()
	player.moveCharacterTo(Vector3(1,0,2))
	await wait_seconds(1)
	assert_onTile(troop.global_position,Vector3(2,1,2))

func test_dontMoveUnSelectedCharacterToATileMovesHim():
	var troop = scene.get_node("Squad/BaseTroop")
	var troopInitialPosition = troop.global_position
	player.moveCharacterTo(Vector3(1,0,2))
	await wait_seconds(1)
	assert_onTile(troop.global_position,troopInitialPosition)

func test_OnlyOneSelectedCharacterAtATime():
	var troop1 = scene.get_node("Squad/BaseTroop")
	var troop2 = scene.get_node("Squad/BaseTroop2")
	troop1.clicked();
	assert_eq(troop2.isSelected(), false)
	assert_eq(troop1.isSelected(), true)
	troop2.clicked()
	assert_eq(troop1.isSelected(), false)
	assert_eq(troop2.isSelected(), true)
	var troop2InitialPosition = troop2.global_position
	troop1.clicked();
	player.moveCharacterTo(Vector3(1,0,2))
	await wait_seconds(1)
	assert_onTile(troop2.global_position,troop2InitialPosition)
	assert_onTile(troop1.global_position,Vector3(2,1,2))

func test_OnlySelectedTroopMoves():
	var troop1 = scene.get_node("Squad/BaseTroop")
	var troop2 = scene.get_node("Squad/BaseTroop2")
	var troop2InitialPosition = troop2.global_position
	troop1.clicked();
	player.moveCharacterTo(Vector3(1,0,2))
	await wait_seconds(1)
	assert_onTile(troop2.global_position,troop2InitialPosition)
	assert_onTile(troop1.global_position,Vector3(2,1,2))
	troop2.clicked()
	player.moveCharacterTo(Vector3(3,0,7))
	await wait_seconds(1)
	assert_onTile(troop2.global_position,Vector3(4,1,8))
	assert_onTile(troop1.global_position,Vector3(2,1,2))

func test_TwoTroopsCanNotMoveToSameLocation():
	var troop1 = scene.get_node("Squad/BaseTroop")
	var troop2 = scene.get_node("Squad/BaseTroop2")
	var troop2Position = troop2.global_position
	var troop1InitialPosition = troop1.global_position
	troop1.clicked();
	player.moveCharacterTo(troop2Position)
	await wait_seconds(1)
	var troop1Position = troop1.global_position
	assert_onTile(troop1Position, troop1InitialPosition)

func test_PreviewAppearsOnMousePositionWhenTroopIsSelected(): 
	var troop1 = scene.get_node("Squad/BaseTroop")
	var squad = scene.get_node("Squad")
	troop1.clicked()
	squad.moveMovementPreview(Vector3(2,0,2))
	var preview = squad.troopPreview
	assert_eq(preview.global_position, Vector3(2,1,2))

func test_PreviewIsShownWhenSelected():
	var troop1 = scene.get_node("Squad/BaseTroop")
	var squad = scene.get_node("Squad")
	troop1.clicked()
	var preview = squad.troopPreview
	assert_ne(preview, null)

func test_PreviewHasNoMeshAtStart():
	var squad = scene.get_node("Squad")
	assert_null(squad.troopPreview)

func test_PreviewHasNoMeshWhenSelectedTroopIsUnselected():
	var squad = scene.get_node("Squad")
	var troop1 = scene.get_node("Squad/BaseTroop")
	
	troop1.clicked()
	troop1.clicked()
	
	var preview = squad.troopPreview
	assert_null(preview)

func test_PreviewHasCorrectMeshDependingOnTroopSelected():
	var squad = scene.get_node("Squad")
	var troop1 = scene.get_node("Squad/BaseTroop")
	var troop2 = scene.get_node("Squad/BaseTroop2")
	var preview = squad.troopPreview
	
	troop1.clicked()
	assert_eq(preview, troop1.getPreviewModel())
	
	troop2.clicked()
	assert_eq(preview, troop2.getPreviewModel())
