import EmployeeArea from "./EmployeeArea.js";
import Fixture from "./Fixture.js";

/**
 * Gets all fixture instances for a floorset.
 * @argument {string} floorsetId
 * @returns {Promise<Fixture[]>}
 *
 * @author Clayton Cook <work@claytonleonardcook.com>
 */
export async function getFixtureInstances(floorsetId) {
  return fetch(
    `${window.location.protocol}//${window.location.hostname}:8085/api/fixtures/get-fixtures-instances/${floorsetId}`,
    {
      method: "GET",
      headers: {
        Authorization: `Bearer ${await getCookie("Auth")}`,
        "Content-Type": "application/json",
      },
      credentials: "include",
    }
  ).then((data) => data.json());
}

/**
 * Gets all employee areas for a floorset.
 * @argument {string} floorsetId
 * @returns {Promise<EmployeeArea[]>}
 *
 * @author Clayton Cook <work@claytonleonardcook.com>
 */
export async function getEmployeeAreas(floorsetId) {
  return fetch(
    `${window.location.protocol}//${window.location.hostname}:8085/api/fixtures/get-employee-areas/${floorsetId}`,
    {
      method: "GET",
      headers: {
        Authorization: `Bearer ${await getCookie("Auth")}`,
        "Content-Type": "application/json",
      },
      credentials: "include",
    }
  ).then((data) => data.json());
}
