import axios from 'axios';
import { jwtDecode } from "jwt-decode"

axios.defaults.baseURL = "http://localhost:5242"

setAthorizitionBearer();
function saveAccsestokon(resConectToken){
    localStorage.setItem("access_token", resConectToken);
    setAthorizitionBearer();
}

function setAthorizitionBearer(){
  const accessToken = localStorage.getItem("access_token");
  if(accessToken)
    axios.defaults.headers.common["Authorization"] = `Bearer ${accessToken}`;
}

axios.interceptors.response.use(
  function (response) {
    return response;
  },
  function (error) {
    if (error.response) {
      if (error.response.status === 401) {
        window.location.href = "/register";
      }}
    return Promise.reject(error);
  }
);


export default {
  getTasks: async () => {
    try{
    const result = await axios.get("/todos")    
    return result.data;
  }
  catch(error){
    console.error("Error fetching tasks:", error);
  }
  },

  addTask: async(name)=>{
    try{
    console.log('addTask', name)
    const result = await axios.post(`/todos`, { taskName :name } ) 
    return result.data;
    }
    catch(error){
      console.error("Error adding task:", error);
    }
  },

  setCompleted: async(id, isComplete)=>{
    try{
    console.log('setCompleted', {id, isComplete})
    const result = await axios.put(`/todos/${id}`, { isComplete:isComplete }) 
    return result.data;
    }
    catch(error){
      console.error("Error updating task:", error);
    }
  },

  deleteTask:async(id)=>{
    try{
    console.log('deleteTask',id)
    const result = await axios.delete(`/todos/${id}`) 
    return result.data;
    }
    catch(error){
      console.error("Error deleting task:", error);
    }
  },
  register:async ( id, username, password) => {
    try {
        const response = await axios.post("/api/register", {
            id,
            username,
            password
        });
        console.log(response.data)
        saveAccsestokon(response.data.token); 
        return response.data;
    } catch (error) {
        console.log("Error registering user:", error);
        throw error;
    }
},
login:async (Id, username, password) => {
    try {
        const response = await axios.post("/api/login", {
            Id,
            username,
            password
        });
        saveAccsestokon(response.data.token); 
        return response.data;
    } catch (error) {
        console.error("Error logging in:", error);
        throw error;
    }
},
getLoginUser: () => {
  try{
  const accessToken = localStorage.getItem("access_token");
  if (accessToken) {
    return jwtDecode(accessToken);
  }
  return null;
}
catch (error) {
  console.error("Error get logging in:", error);
  throw error;
}
},

logout:()=>{
  localStorage.setItem("access_token", "");
},

getPublic: async () => {
  try {
    const res = await axios.get("/api/public");
    if (res && res.status) {
      return res.data;
    } else {
      throw new Error("No response or status");
    }
  } catch (error) {
    console.error("Error fetching public data:", error);
    throw error;
  }
},
getPrivate: async () => {
  try{
    const res = await axios.get("/api/Private");
   return res.data;

  }
  catch (error) {
    console.error("Error get Private in:", error);
    throw error;
}
},
};
