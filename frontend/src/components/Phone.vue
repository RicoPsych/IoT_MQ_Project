<template>
  <div class="phone" :class="{ launched: deviceLauched }" @click="toggleDevice">
    <div class="front">
      <div class="screen" :class="{ launched: deviceLauched }">
        <div class="screen__view">
          <div class="hello">
            <h3>Device number {{ this.selectedDevice }}</h3>
            <p style="height: 40px"></p>
            <p>Altitude</p>
            <p>Last: {{ phoneData.last.altitude }}</p>
            <p>Avg: {{ phoneData.average.altitude }}</p>
            <p style="height: 20px"></p>
            <p>Battery</p>
            <p>Last: {{ phoneData.last.battery }}</p>
            <p>Avg: {{ phoneData.average.battery }}</p>
            <p style="height: 20px"></p>
            <p>Distance</p>
            <p>Last: {{ phoneData.last.distance }}</p>
            <p>Avg: {{ phoneData.average.distance }}</p>
            <p style="height: 20px"></p>
            <p>Temperature</p>
            <p>Last: {{ phoneData.last.temperature }}</p>
            <p>Avg: {{ phoneData.average.temperature }}</p>
          </div>
        </div>
        <div class="screen__front">
          <div class="screen__front-speaker"></div>
          <div class="screen__front-camera"></div>
        </div>
      </div>
    </div>
    <div class="phoneButtons phoneButtons-right"></div>
    <div class="phoneButtons phoneButtons-left"></div>
    <div class="phoneButtons phoneButtons-left2"></div>
    <div class="phoneButtons phoneButtons-left3"></div>
  </div>
</template>

<script>
export default {
  data() {
    return {
      deviceLauched: false,
      interval: null,
      phoneData: {
        last: {
          altitude: 0,
          battery: 0,
          distance: 0,
          temperature: 0,
        },
        average: {
          altitude: 0,
          battery: 0,
          distance: 0,
          temperature: 0,
        },
      },
    };
  },
  props: {
    selectedDevice: 1,
  },
  created() {
    //refresh values once the tracked device is changed
    this.$watch('selectedDevice', () => {
      this.fetchData();
        // let toggle = this.deviceLauched;
        // if(toggle) this.toggleDevice();
        // setTimeout(()=>{
        //   this.fetchData();
        //   if(toggle) this.toggleDevice();
        // },500);
      })

    this.interval = setInterval(() => {
      this.fetchData();
    }, 1000);
  },
  unmounted() {
    clearInterval(this.interval);
  },
  methods: {
    toggleDevice() {
      this.deviceLauched = !this.deviceLauched;
    },
    fetchData() {
      for (const key in this.phoneData.last) {
        fetch(
          `https://localhost:7136/Sensors/GetAvg?types=${key}&instances=${this.selectedDevice}`
        )
          .then((r) => {
            return r.json();
          })
          .then((json) => {
            this.phoneData.average[key] = json.toFixed(3);
            return;
          });
        fetch(
          `https://localhost:7136/Sensors/GetLast?types=${key}&instances=${this.selectedDevice}`
        )
          .then((r) => {
            return r.json();
          })
          .then((json) => {
            this.phoneData.last[key] = json.value.toFixed(3);
            return;
          });
      }
    },
  },
};
</script>

<style scoped>
.phone {
  width: 20em;
  height: 40em;
  display: flex;
  position: absolute;
  transform: rotate(0deg) scale(0.7);
  transition: all cubic-bezier(0.36, 0.41, 0.38, 1) 0.4s;
  z-index: 10;
  transition: all 2s ease;
}
.phone:hover {
  cursor: pointer;
}
.phone.launched {
  transform: rotate(0deg) scale(0.75);
  margin-right: 0%;
}
.phone .front {
  display: flex;
  flex: 1;
  background-color: #292c2d;
  border-radius: 3em;
  margin: 0.2em;
  overflow: hidden;
  position: relative;
  box-shadow: 0 0.1em 0.4em rgba(255, 255, 255, 0.1) inset;
  z-index: 10;
  /* box-shadow: -15px -15px 15px rgba(70, 70, 70, 0.1),
    15px 15px 15px rgb(0, 0, 0, 0.2); */
  box-shadow: 25px 25px 25px rgb(0, 0, 0, 0.2);
}
.phone .screen {
  display: flex;
  flex: 1;
  background-color: #191b1c;
  margin: 0.4em;
  border-radius: 2.6em;
  border: solid 0.2em #121415;
  position: relative;
  z-index: 10;
}
.phone .screen .screen__view {
  display: flex;
  flex: 1;
  margin: 0.6em;
  border-radius: 2em;
  overflow: hidden;
  position: relative;
  width: 100%;
  position: relative;
  align-items: center;
  justify-content: center;
}
.phone .screen .screen__view:after,
.phone .screen .screen__view:before {
  content: "";
  position: absolute;
  z-index: 1;
  width: 50em;
  height: 50em;
  border-radius: 50%;
  background: linear-gradient(
    110deg,
    hsla(160, 100%, 37%, 1),
    #013927,
    rgb(1, 53, 36)
  );
  bottom: 0;
}
.phone .screen .screen__view:after {
  transform: translateY(100%) scaleX(1.4);
  opacity: 0.3;
  transition: all ease 0.5s 0.1s;
}
.phone .screen.launched .screen__view:after {
  transform: translateY(10%) scaleX(1.4);
  transition: all ease 0.9s;
}
.phone .screen .screen__view:before {
  transform: translateY(100%) scaleX(1.4);
  opacity: 1;
  z-index: 40;
  transition: all ease 0.6s;
}
.phone .screen.launched .screen__view:before {
  transform: translateY(10%) scaleX(1.4);
  transition: all ease 0.9s 0.2s;
}

.phone .screen .screen__view .hello {
  font-size: 15px;
  color: #fff;
  position: absolute;
  z-index: 60;
  opacity: 0;
  display: flex;
  flex-direction: column;
  justify-content: center;
  align-items: center;
  transition: all ease 0.3s;
  transform: translateY(40%);
  font-weight: 600;
  font-family: consolas;
}
.phone .screen.launched .screen__view .hello {
  opacity: 1;
  transform: translateY(0%);
  transition: all ease 0.6s 0.7s;
}
.phone .screen .screen__front {
  position: absolute;
  width: 50%;
  background-color: #191b1c;
  height: 1.8em;
  border-radius: 0 0 0.9em 0.9em;
  right: 25%;
  top: 0px;
  display: flex;
  justify-content: center;
  align-items: center;
  padding-bottom: 0.3em;
  box-sizing: border-box;
  margin-top: 0.5em;
  z-index: 999;
}
.phone .screen .screen__front::after,
.phone .screen .screen__front::before {
  content: "";
  width: 10%;
  height: 50%;
  position: absolute;
  background: transparent;
  top: -0.3em;
  border: solid 0.4em #191b1c;
  border-bottom: 0;
}
.phone .screen .screen__front::after {
  left: 0.4em;
  transform: translateX(-100%);
  border-left: 0;
  border-radius: 0 0.7em 0 0;
}
.phone .screen .screen__front::before {
  right: 0.4em;
  transform: translateX(100%);
  border-right: 0;
  border-radius: 0.7em 0 0 0;
}
.phone .screen .screen__front-speaker {
  background: #070808;
  border-radius: 0.2em;
  height: 0.35em;
  width: 28%;
}
.phone .screen .screen__front-camera {
  height: 0.35em;
  width: 0.35em;
  background: #272727;
  margin-left: 0.5em;
  border-radius: 50%;
  margin-right: -0.8em;
}
.phone .phoneButtons {
  width: 1em;
  height: 6em;
  position: absolute;
  z-index: 2;
  background: linear-gradient(to bottom, #212324, #2b2e31, #212324);
  box-shadow: 0 0 0.4em rgba(255, 255, 255, 0.1) inset;
  border-radius: 2px;
}
.phone .phoneButtons-right {
  right: 0;
  top: 30%;
}
.phone .phoneButtons-left {
  left: 0;
  top: 15%;
  height: 3em;
}
.phone .phoneButtons-left2 {
  left: 0;
  top: 25%;
  height: 5em;
}
.phone .phoneButtons-left3 {
  left: 0;
  top: calc(25% + 6em);
  height: 5em;
}
</style>