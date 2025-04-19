import EmployeeArea from "./EmployeeArea.js";
import Fixture from "./Fixture.js";

/**
 * @argument {Fixture} fixture
 * @author Clayton Cook <work@claytonleonardcook.com>
 * @returns {Promise<boolean>}
 */
export async function updateFixtureInstance(fixture) {
  console.log(
    JSON.stringify({
      updateFixture: fixture.toObject(),
    })
  );
  return fetch(
    `${window.location.protocol}//${window.location.host}:8085/api/fixtures/update-fixture-instance`,
    {
      method: "PATCH",
      headers: {
        Authorization: `Bearer ${await getCookie("Auth")}`,
        "Content-Type": "application/json; charset=utf-8",
      },
      credentials: "include",
      body: JSON.stringify(fixture.toObject()),
    }
  ).then((data) => data.ok);
}

/**
 * @argument {Fixture} fixture
 * @author Clayton Cook <work@claytonleonardcook.com>
 * @returns {Promise<boolean>}
 */
export async function createFixtureInstance(fixture) {
  return fetch(
    `${window.location.protocol}//${window.location.hostname}:8085/api/fixtures/create-fixture-instance`,
    {
      method: "POST",
      headers: {
        Authorization: `Bearer ${await getCookie("Auth")}`,
        "Content-Type": "application/json; charset=utf-8",
      },
      credentials: "include",
      body: JSON.stringify(fixture.toObject()),
    }
  ).then((data) => data.ok);
}

/**
 * @argument {EmployeeArea[]} employeeAreas
 * @author Clayton Cook <work@claytonleonardcook.com>
 * @returns {Promise<boolean>}
 */
export async function createEmployeeAreas(employeeAreas) {
  return fetch(
    `${window.location.protocol}//${window.location.hostname}:8085/api/fixtures/add-employee-areas`,
    {
      method: "POST",
      headers: {
        Authorization: `Bearer ${await getCookie("Auth")}`,
        "Content-Type": "application/json; charset=utf-8",
      },
      credentials: "include",
      body: JSON.stringify(employeeAreas),
    }
  ).then((data) => data.ok);
}

/**
 * @argument {EmployeeArea[]} employeeAreas
 * @author Clayton Cook <work@claytonleonardcook.com>
 * @returns {Promise<boolean>}
 */
export async function deleteEmployeeAreas(employeeAreas) {
  return fetch(
    `${window.location.protocol}//${window.location.hostname}:8085/api/fixtures/delete-employee-areas`,
    {
      method: "DELETE",
      headers: {
        Authorization: `Bearer ${await getCookie("Auth")}`,
        "Content-Type": "application/json; charset=utf-8",
      },
      credentials: "include",
      body: JSON.stringify(employeeAreas),
    }
  ).then((data) => data.ok);
}
