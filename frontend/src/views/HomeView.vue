<template>
  <main style="display: flex; flex-direction: column;">

    <div style="display: flex; flex-direction: row; height: 70vh">
      <div class="container left">
        <div class="content top">
          <div>
            <h1 style="color: #00bd7e">Welcome to the Home Page</h1>
            <div style="display: flex">
              <p>the service is currently&nbsp;</p>
              <p v-if="!loading" style="color: #00bd7e">online</p>
              <p v-else style="color: white">offline</p>
            </div>
          </div>
        </div>
        <div class="content bottom">
          <div>
            <h1>Last received measurement</h1>
            <p v-if="!loading">
              at {{ date }} <br />
              Device {{ post.instance }} showing {{ post.type }} of
              {{ post.value.toFixed(3) }} <br />
            </p>
            <p v-else>
              at XX:XX:XX <br />
              Device X showing XXXXXXXX of XXXXXX
            </p>
          </div>
        </div>
      </div>
      <div class="container right" style="flex-direction: row;">
        <div class="content2">
          <p>Select device to track:</p>
          <SelectDevice v-model="selectedDevice"/>
          <p>And tap the phone</p>
        </div>
        <div class="content2">
          <Phone :selectedDevice="selectedDevice"/>
        </div>
      </div>
    </div>
  </main>
</template>

<script>
import dayjs from "dayjs";
import Phone from "../components/Phone.vue";
import SelectDevice from '../components/SelectDevice.vue';

export default {
  data() {
    return {
      loading: true,
      post: null,
      interval: null,
      selectedDevice: 1
    };
  },
  components: {
    Phone,
    SelectDevice,
  },
  created() {
    this.interval = setInterval(() => {
      this.fetchData();
    }, 1000);
  },
  destroyed() {
    clearInterval(this.interval);
  },
  computed: {
    date() {
      return dayjs(this.post.time).format("HH:mm:ss");
    },
  },
  methods: {
    fetchData() {
      try{
        fetch("http://localhost:7136/Sensors/GetLast")
          .then((r) => {
            return r.json();
          })
          .then((json) => {
            // console.log(json);
            if (json != null) this.post = json;
            this.loading = false;
            return;
          });
      } catch (e){
        console.log(e)
      }
    },
  },
};
</script>

<style scoped>
.container {
  margin-left: 0;
  flex-grow: 1;
  flex-basis: 0;
  height: 100%;
  margin: 20px;
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}
.left {
  margin-left: 0;
}
.right {
  box-shadow: inset -5px -5px 5px rgba(70, 70, 70, 0.1),
    inset 5px 5px 5px rgb(0, 0, 0, 0.2);
  margin-right: 0;
}
.content {
  width: 100%;
  height: 50%;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  box-shadow: inset -5px -5px 5px rgba(70, 70, 70, 0.1),
    inset 5px 5px 5px rgb(0, 0, 0, 0.2);
}
.top {
  margin-bottom: 20px;
}
.bottom {
  margin-top: 20px;
}

.content2 {
  width: 50%;
  height: 100%;
  /* background-color: green; */
  display: flex;
  flex-direction: column;
  align-items: center;
  justify-content: center;
}
</style>