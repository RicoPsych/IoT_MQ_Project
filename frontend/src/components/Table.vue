<template>
  <Chart
    v-if="showChart"
    @closeChart="toggleChart"
    :data="data"
  />
  <div class="container">
    <div v-if="loading">
      <h2 style="color: hsla(160, 100%, 37%, 1)">
        Waiting for the server response...
      </h2>
    </div>
    <div v-else>
      <!-- options to filter table content -->
      <div
        style="
          display: flex;
          align-items: center;
          width: 100%;
          justify-content: space-between;
        "
      >
        <form style="display: flex; align-items: center">
          <select class="option-select" v-model="filters.device">
            <option value="-1" disabled selected>device</option>
            <option value="1">1</option>
            <option value="2">2</option>
            <option value="3">3</option>
            <option value="4">4</option>
            <option value="-1">none</option>
          </select>

          <select class="option-select" v-model="filters.parameter">
            <option value="-1" disabled selected>parameter</option>
            <option value="altitude">altitude</option>
            <option value="battery">battery</option>
            <option value="distance">distance</option>
            <option value="temperature">temperature</option>
            <option value="-1">none</option>
          </select>

          <VueDatePicker
            v-model="filters.dateFrom"
            placeholder="date from:"
            dark
            class="date-picker"
            format="yyyy/MM/dd"
          ></VueDatePicker>
          <VueDatePicker
            v-model="filters.dateTo"
            placeholder="date to:"
            dark
            class="date-picker"
            format="yyyy/MM/dd"
          ></VueDatePicker>

          <button type="button" @click="submitFilter">filter</button>
        </form>

        <div style="display: flex; align-items: center">
          <button @click="toggleChart">show chart</button>
          <button @click="downloadCsv">download csv</button>
          <button @click="downloadJson">download json</button>
        </div>
      </div>

      <!-- Table of optionally filtered content -->
      <table>
        <thead>
          <tr>
            <th @click="sort(2)">
              Device
              <Icon icon="bx:up-arrow" :class="{active : isSortArrowActive[4]}" />
              <Icon icon="bx:down-arrow" :class="{active : isSortArrowActive[5]}"/>
            </th>
            <th @click="sort(1)">
              Parameter
              <Icon icon="bx:up-arrow" :class="{active : isSortArrowActive[2]}"/>
              <Icon icon="bx:down-arrow" :class="{active : isSortArrowActive[3]}"/>
            </th>
            <th @click="sort(0)">
              Date
              <Icon icon="bx:up-arrow" :class="{active : isSortArrowActive[0]}"/>
              <Icon icon="bx:down-arrow" :class="{active : isSortArrowActive[1]}"/>
            </th>
            <th @click="sort(3)">
              Value
              <Icon icon="bx:up-arrow" :class="{active : isSortArrowActive[6]}"/>
              <Icon icon="bx:down-arrow" :class="{active : isSortArrowActive[7]}"/>
            </th>
          </tr>
        </thead>
        <tbody>
          <tr v-for="(d, index) in data" :key="index">
            <td>{{ d.instance }}</td>
            <td>{{ d.type }}</td>
            <td>{{ d.time }}</td>
            <td>{{ d.value }}</td>
          </tr>
        </tbody>
      </table>
    </div>
  </div>
</template>

<script>
import VueDatePicker from "@vuepic/vue-datepicker";
import "@vuepic/vue-datepicker/dist/main.css";
import dayjs from "dayjs";
import Chart from "./Chart.vue";
import { Icon } from "@iconify/vue";

export default {
  data() {
    return {
      loading: true,
      data: null,
      showChart: false,

      filters: {
        device: -1,
        parameter: -1,
        dateFrom: null,
        dateTo: null,
      },

      sorting: {
        currentSortBy: 0,
        currentSortOrder: 1,
      },
    };
  },
  components: {
    VueDatePicker,
    Chart,
    Icon,
  },
  computed: {
    isSortArrowActive() {
      // if (this.sorting == null) return null;
      let temp = [];
      for (let i = 0; i < 8; i ++) {
        temp.push(this.sorting.currentSortBy*2 + this.sorting.currentSortOrder == i);
      }
      return temp;
    },
  },
  created() {
    // this.fetchData();
    this.submitFilter();
  },
  methods: {
    // fetchData() {
    //   this.loading = true;
    //   this.data = null;
    //   try {
    //     fetch("http://localhost:7136/Sensors/getAll")
    //       .then((r) => {
    //         return r.json();
    //       })
    //       .then((json) => {
    //         this.data = json.slice(0, 1000);
    //         this.loading = false;
    //         return;
    //       });
    //   } catch (e) {
    //     console.log(e);
    //   }
    // },
    toggleChart() {
      this.showChart = !this.showChart;
    },
    submitFilter() {
      if(this.data == null) this.loading = true;
      // this.data = null;
      let requestParameters = `?By=${this.sorting.currentSortBy}&Order=${this.sorting.currentSortOrder}&Limit=1000`;
      if (this.filters.parameter != -1) {
        requestParameters += "&SensorTypes=" + this.filters.parameter;
      }
      if (this.filters.device != -1) {
        requestParameters += "&Instances=" + this.filters.device;
      }
      if (this.filters.dateFrom != null) {
        requestParameters +=
          "&StartTime=" + dayjs(this.filters.dateFrom).format("YYYY/MM/DD");
      }
      if (this.filters.dateTo != null) {
        requestParameters +=
          "&EndTime=" +
          dayjs(this.filters.dateTo).add(1, "day").format("YYYY/MM/DD");
      }
      console.log(requestParameters)
      try {
        fetch("http://localhost:7136/Sensors/Get" + requestParameters)
          .then((r) => {
            return r.json();
          })
          .then((json) => {
            this.data = json;
            this.loading = false;
            return;
          });
      } catch (e) {
        this.loading = false;
        console.log(e);
      }
    },
    sort(sortBy) {
      if (sortBy === this.sorting.currentSortBy) {
        this.sorting.currentSortOrder =
          this.sorting.currentSortOrder === 1 ? 0 : 1;
      } else{
        this.sorting.currentSortOrder = 0;
      }
      this.sorting.currentSortBy = sortBy;
      // console.log(this.sorting.currentSortBy + "  " + this.sorting.currentSortOrder)
      this.submitFilter();
    },
    downloadCsv() {
      console.log("CSV");
    },
    downloadJson() {
      console.log("JSON");
    },
  },
};
</script>

<style scoped>
.container {
  /* background: yellow; */
  min-height: 70vh;
  margin: 20px 0 20px 0;
  display: flex;
  /* align-items: center; */
  justify-content: center;
  /* box-shadow: inset -5px -5px 5px rgba(70, 70, 70, 0.1),
    inset 5px 5px 5px rgb(0, 0, 0, 0.2); */
  color: var(--color-text);
}

.content {
  width: 100%;
}

table {
  /* background: red; */
  /* border-radius: 50px; */
  /* border-collapse: collapse; */
  width: 100%;
  box-shadow: inset -5px -5px 5px rgba(70, 70, 70, 0.1),
    inset 5px 5px 5px rgb(0, 0, 0, 0.2);
  table-layout: fixed;
  /* min-height: 200px; */
  /* border: 1px solid #bdc3c7; */
}

tr {
  transition: all 0.2s ease-in;
  /* cursor: pointer; */
}

th,
td {
  padding: 5px;
  text-align: left;
  border-bottom: 1px #ddd;
}

th {
  cursor: pointer;
  user-select: none;
}

form {
  padding: 20px 0 20px 0;
}

input {
  outline: none;
  padding: 5px;
  /* border-radius: 50px; */
  background: var(--color-background);
  border: none;
  color: var(--color-accent);
  font-size: 15px;
  /* border: 1px solid hsla(160, 100%, 37%, 1); */
  box-shadow: inset -5px -5px 5px rgba(70, 70, 70, 0.1),
    inset 5px 5px 5px rgb(0, 0, 0, 0.2);
  height: 48px;
}

input:hover {
  /* border: none; */
}

.dp__theme_dark {
  --dp-background-color: var(--color-background);
  --dp-text-color: #fff;
  --dp-hover-color: #484848;
  --dp-hover-text-color: var(--color-accent);
  --dp-hover-icon-color: var(--color-accent);
  --dp-primary-color: var(--color-accent);
  --dp-primary-disabled-color: var(--color-accent);
  --dp-primary-text-color: #fff;
  --dp-secondary-color: #a9a9a9;
  --dp-border-color: #2d2d2d;
  --dp-menu-border-color: #2d2d2d;
  --dp-border-color-hover: var(--color-accent);
}

.date-picker {
  /* --dp-button-height: 87px; Size for buttons in overlays */
  /* min-height: 45px; */
  width: 150px;
  color: var(--color-accent);
  --dp-border-radius: 20px;
}

.option-select {
  font-size: 1rem;
  border-style: none;
  width: 150px;
  /* white-space: normal; */
  text-align: center;
  font-weight: 200;
  /* padding: 16px 14px 18px; */
  color: var(--color-text);
  background-color: var(--color-background);
  height: 37px;
  border: 1px solid #333;
  border-radius: 20px;
}

.option-select:hover {
  transition-duration: 0.5s;
  border: 1px solid var(--color-accent);
}

button {
  height: 37px;
  border: 2px solid hsla(160, 100%, 17%, 1);
  width: 150px;
  background-color: var(--color-background);
  font-size: 1rem;
  color: var(--color-text);
  cursor: pointer;
  border-radius: 20px;
  box-shadow: inset -5px -5px 5px rgba(70, 70, 70, 0.1),
    inset 5px 5px 5px rgb(0, 0, 0, 0.2);
}

button:hover {
  border: 2px solid var(--color-accent);
}

.active {
  color: var(--color-accent);
}
</style>