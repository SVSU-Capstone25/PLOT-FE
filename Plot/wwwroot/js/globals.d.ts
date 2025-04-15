import Fixture from "./Fixture";

interface Window {
    initP5: (elementId: string) => void,
    addFixture: (id: number, x: number, y: number, width: number, length: number) => void,
    updateStoreSize: (width: number, height: number) => void,
    setPaintMode: (enabled: boolean) => void,
    setPlace: () => void,
    setPaint: (paint: string, supercategory_tuid: number) => void,
    createDraggable: (event: DragEvent) => void,
    grid: {
        state: "place" | "paint" | "erase",
        paint: {
            COLOR: string,
            SUPERCATEGORY_TUID?: Fixture["SUPERCATEGORY_TUID"]
        }
    },
    draggedFixture: Pick<Fixture, "WIDTH" | "LENGTH" | "NAME" | "FIXTURE_TUID"> & {
        STORE_TUID: number
    }
  }