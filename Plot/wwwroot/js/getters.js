import Fixture from "./Fixture.js";

/**
 * @argument {string} floorsetId
 * @author Clayton Cook <work@claytonleonardcook.com>
 * @returns {Promise<Fixture[]>}
 */
export async function getFixtureInstances(floorsetId) {
  return fetch(
    `${window.location.origin}:8085/api/fixtures/get-fixtures-instances/${floorsetId}`,
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
