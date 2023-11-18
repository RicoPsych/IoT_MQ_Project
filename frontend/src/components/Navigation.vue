<template>
  <header>
    <nav>
      <h1 class="title">IOT-STATS</h1>
      <ul class="navigation">
        <li><router-link class="link" to="/">home</router-link></li>
        <li><router-link class="link" :to="{ name: 'table' }">table</router-link></li>
        <li><router-link class="link" :to="{ name: 'about' }">about</router-link></li>
      </ul>
    </nav>
  </header>
</template>

<script>
export default {
  name: "Navigation",
};
</script>

<style scoped>
header {
  display: flex;
  justify-content: center;
}

nav {
  width: 100%;
  padding: 20px 0 20px 0;
  position: relative;
  display: flex;
  flex-direction: row;
  margin: 0 auto;
  /* border-image: linear-gradient(to right, red,yellow) 1;
  border-bottom: 1px solid white; */
  /* background-color: red; */
}

.title {
  align-self: center;
  font-size: 25px;
  font-weight: 200;
  letter-spacing: 2px;
  /* background: -webkit-linear-gradient(#000, #00bd7e);
  -webkit-background-clip: text;
  -webkit-text-fill-color: transparent; */
}

.navigation {
  display: flex;
  align-items: center;
  flex: 1;
  justify-content: flex-end;
  list-style: none;
  text-decoration: none;
}

.link {
  font-weight: 500;
  color: #969696;
  list-style: none;
  text-decoration: none;
  display: inline-block;
  text-align: center;
  font-size: 20px;
  letter-spacing: 2px;
  font-weight: 200;
  /* letter-spacing: 2px; */
  padding: 10px 20px 10px 20px;
  margin-left: 20px;
  box-shadow: -2px -2px 2px rgba(70, 70, 70, 0.1), 2px 2px 2px rgb(0, 0, 0, 0.2);
  border-radius: 50px;
}

.link:hover {
  color: hsla(160, 100%, 37%, 1);
}

li a.router-link-exact-active {
  background-color: var(--background1);
  /* border-bottom: 1px solid green; */
  color: hsla(160, 100%, 37%, 1);
  box-shadow: inset -5px -5px 5px rgba(70, 70, 70, 0.1),
    inset 5px 5px 5px rgb(0, 0, 0, 0.2);
  border-radius: 50px;
  /* border: 5px solid hsla(160, 100%, 37%, 1); */
}
</style>